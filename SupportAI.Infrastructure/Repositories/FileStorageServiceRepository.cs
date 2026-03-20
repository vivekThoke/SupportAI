using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupportAI.Application.Interfaces;

namespace SupportAI.Infrastructure.Repositories
{
    public class FileStorageServiceRepository : IFileStorageService
    {
        public async Task<string> SaveAsync(Stream fileStream, string fileName)
        {
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var filePath = Path.Combine(folder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await fileStream.CopyToAsync(stream);

            return filePath;
        }
    }
}
