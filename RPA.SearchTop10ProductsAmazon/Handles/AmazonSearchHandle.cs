using HtmlAgilityPack; // Adiciona o HtmlAgilityPack
using RestSharp;
using RPA.SearchTop10ProductsAmazon.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System;

namespace RPA.SearchTop10ProductsAmazon.Handles
{
    public class AmazonSearchHandle
    {
        private readonly RestClient _client;
        private readonly string _baseUrl;

        public AmazonSearchHandle(IConfiguration configuration)
        {
            _baseUrl = configuration["Amazon:BaseUrl"];

            // Verifica se o valor da URL base não é nulo ou vazio
            if (string.IsNullOrEmpty(_baseUrl))
            {
                throw new ArgumentNullException(nameof(_baseUrl), "A URL base da Amazon não pode ser nula ou vazia.");
            }

            _client = new RestClient(_baseUrl);
        }

        // Método que realiza a busca e retorna os 10 primeiros resultados
        public async Task<List<Produto>> SearchTop10Products(string searchTerm)
        {
            var produtos = new List<Produto>();

            // Verifica se o termo de busca é válido
            if (string.IsNullOrEmpty(searchTerm))
            {
                throw new ArgumentNullException(nameof(searchTerm), "O termo de busca não pode ser nulo ou vazio.");
            }

            // Cria a requisição HTTP
            var request = new RestRequest("/s", Method.Get);
            request.AddParameter("k", searchTerm);

            // Faz a requisição
            var response = await _client.ExecuteAsync(request);

            // Verifica se a resposta foi bem sucedida
            if (response.IsSuccessful && response.Content != null)
            {
                // Carrega o HTML da resposta na classe HtmlDocument
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(response.Content);

                // Extrai os dados dos produtos do HTML (ajustar os seletores conforme a estrutura da página)
                var productNodes = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 's-result-item')]");

                if (productNodes != null)
                {
                    int count = 0;
                    foreach (var productNode in productNodes)
                    {
                        if (count >= 10) break;

                        // Extrai o nome do produto
                        var nomeNode = productNode.SelectSingleNode(".//span[contains(@class, 'a-size-medium')]");
                        string nome = nomeNode?.InnerText.Trim() ?? "Nome não encontrado";

                        // Extrai o preço (ajuste conforme a estrutura HTML)
                        var valorNode = productNode.SelectSingleNode(".//span[@class='a-offscreen']");
                        decimal? valor = valorNode != null ? decimal.Parse(valorNode.InnerText.Replace("$", "")) : (decimal?)null;

                        // Extrai o link do produto
                        var linkNode = productNode.SelectSingleNode(".//a[@class='a-link-normal']");
                        string url = linkNode != null ? _baseUrl + linkNode.GetAttributeValue("href", "") : "Link não encontrado";

                        // Adiciona o produto à lista
                        produtos.Add(new Produto
                        {
                            Nome = nome,
                            Valor = valor,
                            QuantidadeVendida = null, // A quantidade vendida pode não estar disponível
                            Url = url
                        });

                        count++;
                    }
                }
            }
            else
            {
                // Lida com a situação de uma resposta não bem-sucedida
                throw new InvalidOperationException($"Falha na requisição de pesquisa da Amazon: {response.ErrorMessage}");
            }

            return produtos;
        }
    }
}
