using RPA.SearchTop10ProductsAmazon.Handles;
using RPA.SearchTop10ProductsAmazon.Models;
using RPA.SearchTop10ProductsAmazon.Data;

namespace RPA.SearchTop10ProductsAmazon.Workers
{
    public class SearchWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly string _outputFilePath = "Resultados.xlsx"; 

        public SearchWorker(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                // Criando o escopo para os serviços scoped
                var amazonSearchHandle = scope.ServiceProvider.GetRequiredService<AmazonSearchHandle>();
                var excelHandle = scope.ServiceProvider.GetRequiredService<ExcelHandle>();
                var produtoRepository = scope.ServiceProvider.GetRequiredService<ProdutoRepository>();

                // Lê as palavras do arquivo Excel 
                string _intputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "RPA.SearchTop10ProductsAmazon", "ProductsForSearch.xlsx");
                var searchTerms = excelHandle.ReadSearchTerms("_intputFilePath");

                var produtosResultados = new List<Produto>();

                foreach (var term in searchTerms)
                {
                    // Para cada termo de busca, realiza a pesquisa na Amazon
                    var results = await amazonSearchHandle.SearchTop10Products(term);
                    produtosResultados.AddRange(results);       
                }

                // Salvar os resultados no arquivo Excel
                excelHandle.SaveProductsToExcel(produtosResultados, _outputFilePath);

                // Salvar os resultados no banco de dados
                await produtoRepository.AddProdutosAsync(produtosResultados);

                Environment.Exit(0); // Finalizando a aplicação
            }
        }
    }
}
