namespace RPA.SearchTop10ProductsAmazon.Models
{
    public class Produto
    {
        // Nome do produto encontrado
        public string Nome { get; set; }

        // Valor do produto
        public decimal? Valor { get; set; }

        // Quantidade vendida no último mês (se disponível)
        public int? QuantidadeVendida { get; set; }

        // URL do produto na Amazon
        public string Url { get; set; }
    }
}
