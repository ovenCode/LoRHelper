using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace desktop
{
    public class LoRApiWorker : BackgroundService
    {
        private readonly ILogger<LoRApiWorker> _logger;
        private Task? task;

        public LoRApiWorker(ILogger<LoRApiWorker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                if (task != null)
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        _logger.LogInformation("Retrieving information at {time}", DateTimeOffset.Now);
                        await task.WaitAsync(stoppingToken);                        
                        await Task.Delay(1300);
                    }
                }
                else
                {
                    throw new NullReferenceException("Worker Task is null");
                }
            }            
            catch (Exception)
            {

                throw;
            }
            
            throw new NotImplementedException();
        }

        public void SetTask(Task newTask)
        {
            task = newTask;
        }
    }
}
