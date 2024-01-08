using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YellowCanaryLibrary.Services.Implementations;
using YellowCanaryLibrary.Services.Interfaces;

class Program
{

    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Super Application Starting...\n");

            var host = CreateHostBuilder(args).Build();
            host.Services.GetRequiredService<SuperApplication>().Run();

            Console.WriteLine("Super Application Finished...");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        Console.ReadLine();
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices(ConfigureServices);

    static void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<SuperApplication>();
        services.AddScoped<ISuperDataReader, ExcelSuperDataReader>();
        services.AddScoped<ISuperService, SuperService>();
        services.AddScoped<IReportGenerator, ConsoleReportGenerator>();
    }

}