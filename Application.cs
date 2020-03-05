using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIConsoleApp1
{
    public class Application : IApplication
    {
        ILogger<Application> _logger;

        public Application(ILogger<Application> logger)
        {
            _logger = logger;
        }


        public async Task RunAsync()
        {

            foreach(var i in Enumerable.Range(1, 100))
            {
                _logger.LogInformation("Hello World From Logger!");
                await Task.Delay(1000);
            }

            await Task.CompletedTask;
        }
    }
}
