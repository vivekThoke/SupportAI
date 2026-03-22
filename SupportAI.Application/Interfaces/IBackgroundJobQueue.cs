using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportAI.Application.Interfaces
{
    public interface IBackgroundJobQueue
    {
        void Enqueue(Guid documentId);
        Task<Guid> DequeueAsync(CancellationToken token);
    }
}
