using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SupportAI.Application.Interfaces;
using SupportAI.Infrastructure.AI;
using SupportAI.Infrastructure.Identity;
using SupportAI.Infrastructure.Persistence;
using SupportAI.Infrastructure.Processing;
using SupportAI.Infrastructure.Repositories;
using SupportAI.Infrastructure.Security;

namespace SupportAI.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddHttpClient();

            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITenantRepository, TenantRepository>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IFileStorageService, FileStorageServiceRepository>();
            services.AddScoped<DocumentProcessor>();
            services.AddScoped<TextExtractor>();
            services.AddScoped<TextChunker>();
            services.AddScoped<IDocumentChunkRepository, DocumentChunkRepository>();
            services.AddHostedService<DocumentProcessingService>();
            services.AddScoped<IEmbeddingService, GeminiEmbeddingService>();
            services.AddScoped<IVectorDatabase, QdrantService>();

            return services;
        }
    }
}
