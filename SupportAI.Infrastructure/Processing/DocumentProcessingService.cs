using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;
using SupportAI.Application.Interfaces;

namespace SupportAI.Infrastructure.Processing
{
    public class DocumentProcessingService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IBackgroundJobQueue _backgroundJobQueue;

        public DocumentProcessingService(IServiceProvider serviceProvider, IBackgroundJobQueue backgroundJobQueue)
        {
            _serviceProvider = serviceProvider;
            _backgroundJobQueue = backgroundJobQueue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var documentId = await _backgroundJobQueue.DequeueAsync(stoppingToken);

                try
                {
                    using var scope = _serviceProvider.CreateScope();

                    var processor = scope.ServiceProvider.GetRequiredService<DocumentProcessor>();

                    await processor.ProcessSingleAsync(documentId);
                }
                catch (Exception ex)
                {
                    
                }
            }
        }
    }
}
