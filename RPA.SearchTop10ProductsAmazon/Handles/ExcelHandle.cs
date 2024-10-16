using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;

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
    }
}
