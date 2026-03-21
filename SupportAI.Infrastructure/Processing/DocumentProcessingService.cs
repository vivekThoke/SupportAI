using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;

namespace SupportAI.Infrastructure.Processing
{
    public class DocumentProcessingService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public DocumentProcessingService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();

                var processor = scope.ServiceProvider.GetRequiredService<DocumentProcessor>();

                await processor.ProcessAsync();

                await Task.Delay(500, stoppingToken);
            }
        }
    }
}
