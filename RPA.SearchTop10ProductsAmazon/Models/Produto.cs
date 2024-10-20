namespace RPA.SearchTop10ProductsAmazon.Models
{
    public class Produto
    {
        public int Id { get; set; } // Chave Primária (caso você vá usar um banco de dados)
        public string Nome { get; set; }
        public string Valor { get; set; }
        public string QuantidadeVendida { get; set; }
        public string Url { get; set; }
        public int QuantidadeVendidaNumerica { get; set; } // Adicionada para armazenar a quantidade como número
    }
}
