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
            var connectionString = _configuration.GetConnectionString("AzureBlobStorage");

            if (_env.IsDevelopment())
            {
                CreateContainerIfNotExists(connectionString);
            }

            services.AddAzureMkFileStorage(_configuration, connectionString);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            { 
                endpoints.MapMkFileStorage();
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
