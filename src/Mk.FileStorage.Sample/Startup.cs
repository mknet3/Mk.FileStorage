using Azure.Storage.Blobs;
using Mk.FileStorage.Azure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Mk.FileStorage.Sample
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _env;

        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            if (_env.IsDevelopment())
            {
                CreateContainerIfNotExists();
            }

            services.Configure<AzureBlobStorageOptions>(_configuration.GetSection(AzureBlobStorageOptions.DefaultSection));
            services.AddTransient(serviceProvider => new BlobServiceClient(_configuration["AzureBlobStorage:ConnectionString"]));
            services.AddTransient<IFileStorageService, AzureBlobStorageService>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            { 
                endpoints.MapFileStorage();
            });
        }

        private void CreateContainerIfNotExists()
        {
            var section = AzureBlobStorageOptions.DefaultSection;
            var connectionString = _configuration[$"{section}:{nameof(AzureBlobStorageOptions.ConnectionString)}"];
            var containerName = _configuration[$"{section}:{nameof(AzureBlobStorageOptions.ContainerName)}"];

            var blobServiceClient = new BlobServiceClient(connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            blobContainerClient.CreateIfNotExists();
        }
    }
}
