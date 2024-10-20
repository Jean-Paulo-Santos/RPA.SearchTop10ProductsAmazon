using Microsoft.Extensions.DependencyInjection; // Necessário para criar escopos
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RPA.SearchTop10ProductsAmazon.Handles;
using RPA.SearchTop10ProductsAmazon.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RPA.SearchTop10ProductsAmazon.Data;

namespace RPA.SearchTop10ProductsAmazon.Workers
{
    public class SearchWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly string _outputFilePath = "Resultados.xlsx"; // Caminho do arquivo de saída

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
                var searchTerms = excelHandle.ReadSearchTerms("C:\\Users\\Cliente\\source\\repos\\RPA.SearchTop10ProductsAmazon\\ProductsForSearch.xlsx");

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
