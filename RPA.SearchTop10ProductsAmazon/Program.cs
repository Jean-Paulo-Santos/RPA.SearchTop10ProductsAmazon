using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RPA.SearchTop10ProductsAmazon.Data;
using RPA.SearchTop10ProductsAmazon.Handles;
using RPA.SearchTop10ProductsAmazon.Workers;

namespace RPA.SearchTop10ProductsAmazon
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            // Configurando o DatabaseContext
            services.AddDbContext<DatabaseContext>(options =>
                options.UseNpgsql("Host=localhost;Database=AmazonProductsDB;Username=postgres;Password=Jc141094."));

            // Registrando outros serviços como scoped
            services.AddScoped<ProdutoRepository>();
            services.AddScoped<AmazonSearchHandle>();
            services.AddScoped<ExcelHandle>();

            // Registrando o SearchWorker como HostedService
            services.AddHostedService<SearchWorker>();
        });
    }
}