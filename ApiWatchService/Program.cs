using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ApiWatchService
{
    public class Program
    {
        public static void Main(string[] args)
        {

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory)
                                        .AddJsonFile("host.json")
                                        .AddJsonFile("appsettings.json")
                                        .AddJsonFile("appsettings.Development.json")
                                        .AddJsonFile("ApiWatchInfos.json")
                                        .Build();
            return WebHost.CreateDefaultBuilder(args)
                            .UseConfiguration(configuration)
                            .UseStartup<Startup>();
        }
    }
}
