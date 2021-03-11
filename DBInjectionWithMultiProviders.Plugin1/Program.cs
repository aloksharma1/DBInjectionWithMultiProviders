using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBInjectionWithMultiProviders.Plugin1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        //https://stackoverflow.com/questions/61379693/asp-net-core-kestrel-port-sharing-alternative
        //.ConfigureWebHost(webHost=> {    
        //    //We are sharing port with .net HTTPListener object with streaming service on SF cluster so we need to use old HTTP.sys and we can not use new Kestrel
        //    webHost.UseHttpSys(options =>
        //    {
        //        options.UrlPrefixes.Add(urlPrefix);
        //    });
        //});
    }
}
