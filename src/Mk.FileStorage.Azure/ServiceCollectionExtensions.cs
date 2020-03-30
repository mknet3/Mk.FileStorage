using System;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Mk.FileStorage;
using Mk.FileStorage.Azure;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAzureMkFileStorage(
            this IServiceCollection services,
            IConfiguration configuration,
            Func<IServiceProvider, BlobServiceClient> configureBlobClient)
        {
            services.Configure<AzureBlobStorageOptions>(configuration.GetSection(AzureBlobStorageOptions.DefaultSection));
            services.AddTransient(configureBlobClient);
            services.AddTransient<IFileStorageService, AzureBlobStorageService>();
            return services;
        }

        public static IServiceCollection AddAzureMkFileStorage(
            this IServiceCollection services,
            IConfiguration configuration,
            string connectionString)
        {
            services.Configure<AzureBlobStorageOptions>(configuration.GetSection(AzureBlobStorageOptions.DefaultSection));
            services.AddTransient(serviceProvider => new BlobServiceClient(connectionString));
            services.AddTransient<IFileStorageService, AzureBlobStorageService>();
            return services;
        }
    }
}
