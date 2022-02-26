using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PeopleData.Business.Services;
using PeopleData.Business.Services.Implementation;
using PeopleData.ConsoleApp;
using PeopleData.Data.Settings;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PeopleDataApp.ConsoleApp
{
    class Program
    {

        private static IConfiguration Configuration { get; set; }

        static async Task Main(string[] args)
        {

             Configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: true)
             .Build();


            var host = CreateHostBuilder(args).
                ConfigureServices(ConfigureServices)
               .ConfigureLogging(logger =>
               {
                   logger.AddConsole();
               })
               .Build();
                
           
            var app = host.Services.GetService<App>();
            await app.Start();




        }


        private static void ConfigureServices(IServiceCollection service)
        {
            // addServices
            service.AddScoped<App>();
            service.AddScoped<IPersonService, PersonService>();
            service.Configure<ODataConfig>(Configuration.GetSection("ODataConfig"));

        }


        private static IHostBuilder CreateHostBuilder(string [] args) {
            return Host.CreateDefaultBuilder(args);
        }
    }
}
