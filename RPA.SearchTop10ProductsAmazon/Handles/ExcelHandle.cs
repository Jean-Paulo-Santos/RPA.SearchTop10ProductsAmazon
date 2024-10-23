using OfficeOpenXml;
using RPA.SearchTop10ProductsAmazon.Models;


namespace RPA.SearchTop10ProductsAmazon.Handles
{
    public class ExcelHandle
    {
        public List<string> ReadSearchTerms(string filePath)
        {
            var searchTerms = new List<string>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Carregar o arquivo Excel
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                // Acessar a primeira planilha
                var worksheet = package.Workbook.Worksheets[0];

                // Ler as células com os termos de busca
                int row = 1;
                while (worksheet.Cells[row, 1].Value != null)
                {
                    searchTerms.Add(worksheet.Cells[row, 1].Value.ToString());
                    row++;
                }
            }

            return searchTerms;
        }

        public void SaveProductsToExcel(List<Produto> produtos, string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Resultados");
                worksheet.Cells[1, 1].Value = "Nome";
                worksheet.Cells[1, 2].Value = "Valor";
                worksheet.Cells[1, 3].Value = "Quantidade Vendida";
                worksheet.Cells[1, 4].Value = "URL";

                for (int i = 0; i < produtos.Count; i++)
                {
                    var produto = produtos[i];
                    worksheet.Cells[i + 2, 1].Value = produto.Nome;
                    worksheet.Cells[i + 2, 2].Value = produto.Valor;
                    worksheet.Cells[i + 2, 3].Value = produto.QuantidadeVendida;
                    worksheet.Cells[i + 2, 4].Value = produto.Url;
                }

                package.SaveAs(new FileInfo(filePath));
            }
        }
    }
}
