using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RPA.SearchTop10ProductsAmazon.Data; // Importa o namespace correto
using RPA.SearchTop10ProductsAmazon.Handles; // Importa o namespace correto
using System.Threading;
using System.Threading.Tasks;

namespace RPA.SearchTop10ProductsAmazon.Workers
{
    public class SearchWorker : BackgroundService
    {
        private readonly AmazonSearchHandle _amazonSearchHandle;
        private readonly ExcelHandle _excelHandle;
        private readonly PostgresDbContext _postgresDbContext; // Declare o DbContext

        public SearchWorker(AmazonSearchHandle amazonSearchHandle, ExcelHandle excelHandle, PostgresDbContext postgresDbContext)
        {
            _amazonSearchHandle = amazonSearchHandle;
            _excelHandle = excelHandle;
            _postgresDbContext = postgresDbContext; // Inicialize o DbContext
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Lê as palavras do arquivo Excel
            var searchTerms = _excelHandle.ReadSearchTerms("search_terms.xlsx");

            foreach (var term in searchTerms)
            {
                // Para cada termo de busca, realiza a pesquisa na Amazon
                var results = await _amazonSearchHandle.SearchTop10Products(term);

                // Salvar os resultados no banco de dados
                foreach (var product in results)
                {
                    await _postgresDbContext.SaveProductAsync(product); // Chamada correta ao método
                }
            }
        }
    }
}
