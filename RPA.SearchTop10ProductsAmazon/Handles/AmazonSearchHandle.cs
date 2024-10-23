using HtmlAgilityPack;
using RestSharp;
using RPA.SearchTop10ProductsAmazon.Models;


namespace RPA.SearchTop10ProductsAmazon.Handles
{
    public class AmazonSearchHandle
    {
        private readonly RestClient _client;
        private readonly string _baseUrl;

        public AmazonSearchHandle(IConfiguration configuration)
        {
            _baseUrl = configuration["Amazon:BaseUrl"];
            _client = new RestClient(_baseUrl);
        }

        // Método que realiza a busca e retorna os 10 primeiros resultados
        public async Task<List<Produto>> SearchTop10Products(string searchTerm)
        {
            var produtos = new List<Produto>();

            // Cria a requisição HTTP
            var request = new RestRequest("/s", Method.Get);
            request.AddParameter("k", searchTerm);

            // Faz a requisição
            var response = await _client.ExecuteAsync(request);

            // Verifica se a resposta foi bem sucedida
            if (response.IsSuccessful)
            {
                // Carrega o HTML da resposta na classe HtmlDocument
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(response.Content);

                // Extrai os dados dos produtos do HTML 
                var productNodes = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class,'sg-col-4-of-24 sg-col-4-of-12 s-result-item s-asin sg-col-4-of-16')]");

                if (productNodes != null)
                {
                    foreach (var productNode in productNodes)
                    {
                        // Extrai o nome do produto 
                        var nomeNode = productNode.SelectSingleNode(".//span[contains(@class,'a-size-base-plus a-color-base a-text-normal')]");
                        string nome = nomeNode?.InnerText.Trim() ?? "Nome não encontrado";

                        // Extrai o preço
                        var valorNode = productNode.SelectSingleNode(".//span[@class='a-offscreen']");
                        string valor = valorNode != null ? valorNode.InnerText.Trim() : "Preço não encontrado";

                        // Extrai a quantidade vendida no ultimo mês
                        var qtdNode = productNode.SelectSingleNode(".//span[@class='a-size-base a-color-secondary']");
                        string qtdv = qtdNode?.InnerText.Trim();

                        // Inicializa a quantidade vendida
                        int quantidadeVendida = 0;

                        if (qtdv != null && qtdv.Contains("Mais de"))
                        {
                            // Extraímos o número da string, por exemplo: "Mais de 500 compras"
                            var quantidadeStr = qtdv.Split(' ')[2]; // Pega a quantidade (ex.: "500")
                            int.TryParse(quantidadeStr, out quantidadeVendida); // Converte para inteiro
                        }
                        else
                        {
                            // Caso contrário, manter a quantidade como 0 para ordenar corretamente
                            qtdv = "Informação não encontrada";
                        }

                        // Extrai o link do produto
                        var linkNode = productNode.SelectSingleNode(".//a[@class='a-link-normal s-underline-text s-underline-link-text s-link-style a-text-normal']");
                        string url = linkNode != null ? _baseUrl + linkNode.GetAttributeValue("href", "") : "Link não encontrado";

                        // Adiciona o produto à lista
                        produtos.Add(new Produto
                        {
                            Nome = nome,
                            Valor = valor,
                            QuantidadeVendida = qtdv,
                            Url = url,
                            QuantidadeVendidaNumerica = quantidadeVendida 
                        });
                    }
                }
            }

            // Separa produtos com e sem quantidade válida
            var produtosComQuantidade = produtos.Where(p => p.QuantidadeVendidaNumerica > 0).OrderByDescending(p => p.QuantidadeVendidaNumerica).ToList();
            var produtosSemQuantidade = produtos.Where(p => p.QuantidadeVendidaNumerica == 0).ToList();

            // Combina as listas, primeiro com quantidade e depois sem quantidade
            var produtosClassificados = produtosComQuantidade.Concat(produtosSemQuantidade).Take(10).ToList();

            return produtosClassificados; // Retorna a lista classificada
        }
    }
}
