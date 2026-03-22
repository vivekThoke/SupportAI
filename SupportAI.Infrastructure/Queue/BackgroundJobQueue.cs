using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using SupportAI.Application.Interfaces;

namespace SupportAI.Infrastructure.Queue
{
    public class BackgroundJobQueue : IBackgroundJobQueue
    {
        private readonly Channel<Guid> _queue;

        public BackgroundJobQueue()
        {
            _queue = Channel.CreateUnbounded<Guid>(new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = false
            });
        }

        public void Enqueue(Guid documentId)
        {
            if (!_queue.Writer.TryWrite(documentId))
            {
                throw new Exception("Failed to Enqueue job");
            }
        }

        public async Task<Guid> DequeueAsync(CancellationToken token)
        {
            return await _queue.Reader.ReadAsync(token);
        }
    }
}
