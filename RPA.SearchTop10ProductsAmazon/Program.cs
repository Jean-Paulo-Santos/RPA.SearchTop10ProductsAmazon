using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RPA.SearchTop10ProductsAmazon.Data;
using RPA.SearchTop10ProductsAmazon.Handles;
using RPA.SearchTop10ProductsAmazon.Workers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = WebApplication.CreateBuilder(args);

// Configura��o do banco de dados
builder.Services.AddScoped<PostgresDbContext>();

// Adiciona as depend�ncias necess�rias
builder.Services.AddScoped<AmazonSearchHandle>();
builder.Services.AddScoped<ExcelHandle>();

// Registra o SearchWorker como um IHostedService
builder.Services.AddHostedService(provider =>
{
    // Cria um escopo para resolver os servi�os scoped
    var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
    return new SearchWorker(
        scopeFactory.CreateScope().ServiceProvider.GetRequiredService<AmazonSearchHandle>(),
        scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ExcelHandle>(),
        scopeFactory.CreateScope().ServiceProvider.GetRequiredService<PostgresDbContext>()
    );
});

var app = builder.Build();

// Configura��o do aplicativo
app.MapGet("/", () => "API Running");

// Inicia o aplicativo
app.Run();
