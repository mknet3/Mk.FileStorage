using System;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mk.FileStorage;
using Mk.FileStorage.Azure;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAzureMkFileStorage(
            this IServiceCollection services,
            IConfigurationSection configuration,
            Func<IServiceProvider, BlobServiceClient> configureBlobClient)
        {
            services.Configure<AzureBlobStorageOptions>(configuration);
            services.TryAddScoped(configureBlobClient);
            services.TryAddScoped<IFileStorageService, AzureBlobStorageService>();
            return services;
        }

        public static IServiceCollection AddAzureMkFileStorage(
            this IServiceCollection services,
            IConfigurationSection configuration,
            string connectionString)
        {
            services.Configure<AzureBlobStorageOptions>(configuration);
            services.TryAddScoped(serviceProvider => new BlobServiceClient(connectionString));
            services.TryAddScoped<IFileStorageService, AzureBlobStorageService>();
            return services;
        }


    }
}
