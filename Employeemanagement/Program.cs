using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
namespace Employeemanagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
           BuildWebHost(args).Run();
        }
     
    

        public static IWebHost BuildWebHost(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
             .UseStartup<Startup>()
             .ConfigureLogging((hostingContext, logging) =>
             {
               // The ILoggingBuilder minimum level determines the
               // the lowest possible level for logging. The log4net
               // level then sets the level that we actually log at.
               //logging.AddDebug();
               //  logging.SetMinimumLevel(LogLevel.Debug);
                 logging.ClearProviders(); // removes all providers from LoggerFactory
                 logging.AddConsole();
                 logging.AddTraceSource("Information, ActivityTracing");
                
             })
              .Build();
    }
}
