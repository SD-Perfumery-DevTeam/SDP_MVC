using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog.Web;

namespace SDP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Adding logging with NLog.
            // reference: https://github.com/NLog/NLog/wiki/Getting-started-with-ASP.NET-Core-5
            var logger = NLogBuilder.ConfigureNLog("nlog.config")
                .GetCurrentClassLogger();
            try
            {
                logger.Debug("Initialise Main.");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                // Log any errors in startup.
                logger.Error(ex, "An exception occured when initialising Main.");
            }
            finally
            {
                // Flush and stop internal timers / threads (avoids segmentation fault).
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Global.Startup>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog(); // Setup NLog for dependency injection.
    }
}
