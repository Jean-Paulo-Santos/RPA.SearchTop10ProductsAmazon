using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                    services.AddSingleton<AmazonSearchHandle>();
                    services.AddSingleton<ExcelHandle>();
                    services.AddHostedService<SearchWorker>();
                });
    }
}
