using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace PeopleDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
             CreateHostBuilder(args).
                ConfigureServices(ConfigureServices)
                .Build();
        }


        private static void ConfigureServices(IServiceCollection service)
        {
            // addServices

        }

        private static IHostBuilder CreateHostBuilder(string [] args) {
            return Host.CreateDefaultBuilder(args)
        }
    }
}
