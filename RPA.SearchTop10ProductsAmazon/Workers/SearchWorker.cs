using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RPA.SearchTop10ProductsAmazon.Handles;
using RPA.SearchTop10ProductsAmazon.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RPA.SearchTop10ProductsAmazon.Workers
{
    public class SearchWorker : BackgroundService
    {
        private readonly AmazonSearchHandle _amazonSearchHandle;
        private readonly ExcelHandle _excelHandle;
        private readonly string _outputFilePath = "Resultados.xlsx"; // Caminho do arquivo de saída

        public SearchWorker(AmazonSearchHandle amazonSearchHandle, ExcelHandle excelHandle)
        {
            _amazonSearchHandle = amazonSearchHandle;
            _excelHandle = excelHandle;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Lê as palavras do arquivo Excel
            var searchTerms = _excelHandle.ReadSearchTerms("C:\\Users\\Cliente\\source\\repos\\RPA.SearchTop10ProductsAmazon\\ProductsForSearch.xlsx");

            var produtosResultados = new List<Produto>();

            foreach (var term in searchTerms)
            {
                // Para cada termo de busca, realiza a pesquisa na Amazon
                var results = await _amazonSearchHandle.SearchTop10Products(term);
                produtosResultados.AddRange(results);
            }

            // Salvar os resultados no arquivo Excel
            _excelHandle.SaveProductsToExcel(produtosResultados, _outputFilePath);
        }
    }
}
