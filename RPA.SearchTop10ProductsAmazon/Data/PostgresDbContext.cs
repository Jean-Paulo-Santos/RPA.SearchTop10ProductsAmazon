using Npgsql;
using Microsoft.Extensions.Configuration;
using RPA.SearchTop10ProductsAmazon.Models;
using System.Threading.Tasks;

namespace RPA.SearchTop10ProductsAmazon.Data
{
    public class PostgresDbContext
    {
        private readonly string _connectionString;

        public PostgresDbContext(IConfiguration configuration)
        {
            // Recupera a string de conexão do arquivo de configuração (appsettings.json)
            _connectionString = configuration.GetConnectionString("PostgresConnection");
        }

        // Método para salvar o produto no banco de dados
        public async Task SaveProductAsync(Produto produto)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Comando SQL para inserir o produto no banco de dados
                var query = @"
                    INSERT INTO produtos (nome, valor, quantidade_vendida, url)
                    VALUES (@nome, @valor, @quantidade_vendida, @url);
                ";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    // Adiciona os parâmetros da consulta
                    command.Parameters.AddWithValue("nome", produto.Nome);
                    command.Parameters.AddWithValue("valor", (object)produto.Valor ?? DBNull.Value);
                    command.Parameters.AddWithValue("quantidade_vendida", (object)produto.QuantidadeVendida ?? DBNull.Value);
                    command.Parameters.AddWithValue("url", produto.Url);

                    // Executa o comando de inserção
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
