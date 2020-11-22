using Azure.Storage.Blobs;
using Mk.FileStorage.Azure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mk.FileStorage.AspNetCore;

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
            var connectionString = _configuration.GetConnectionString("AzureBlobStorage");

            if (_env.IsDevelopment())
            {
                CreateContainerIfNotExists(connectionString);
            }

            var section = _configuration.GetSection(AzureBlobStorageOptions.DefaultSection);

            // Register Azure Blob as main File Storage
            services.AddAzureMkFileStorage(section!, connectionString!);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                // Endpoint to download and upload stream files to registered storage (in this case Azure)
                // Local example: https://localhost:5001/my-files/fileName.txt
                endpoints.MapMkFileStorage("my-files");
            });
        }

        private void CreateContainerIfNotExists(string connectionString)
        {
            var section = AzureBlobStorageOptions.DefaultSection;
            var containerName = _configuration.GetSection(section)
                .GetValue<string>(nameof(AzureBlobStorageOptions.ContainerName));

            var blobServiceClient = new BlobServiceClient(connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            blobContainerClient.CreateIfNotExists();
        }
    }
}
