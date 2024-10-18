using HtmlAgilityPack;
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

                // Extrai os dados dos produtos do HTML (ajustar os seletores conforme a estrutura da página)
                var productNodes = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class,'sg-col-4-of-24 sg-col-4-of-12 s-result-item s-asin sg-col-4-of-16')]");

                if (productNodes != null)
                {
                    int count = 0;
                    foreach (var productNode in productNodes)
                    {
                        if (count >= 10) break;

                        // Extrai o nome do produto 
                        var nomeNode = productNode.SelectSingleNode(".//span[contains(@class,'a-size-base-plus a-color-base a-text-normal')]");
                        string nome = nomeNode?.InnerText.Trim() ?? "Nome não encontrado";

                        // Extrai o preço (ajuste conforme a estrutura HTML)
                        var valorNode = productNode.SelectSingleNode(".//span[@class='a-price-whole']");
                        string valor = valorNode != null ? valorNode.InnerText.Trim() : "Preço não encontrado";


                        var qtdNode = productNode.SelectSingleNode(".//span[@class='a-size-base a-color-secondary']");
                        string qtdv = qtdNode?.InnerText.Trim() ?? "Informação não encontrada";

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
                        });

                        count++;
                    }
                }
            }

            return produtos;
        }
    }
}
