# Mk

Mk is a simple solution to integrate a File Storage in your ASP.NET Core project.

![CI](https://github.com/m-knet/Mk.FileStorage/workflows/CI/badge.svg)

## Getting Started

### Prerequisites

* .NET Core 3.1
* Docker

### Development Environment

We can use Azurite to emulate an Azure Storage. We can run this dependency with docker-compose:

```bash
docker-compose up
```

## Sample

To be able to upload files from your project to Azure Storage only you need add this endpoint in the Startup:

```csharp
    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapFileStorage();
        });
    }
```

And register an IFileStorageService. In this case we add Azure Blob implementation:

```csharp
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AzureBlobStorageOptions>(_configuration.GetSection(AzureBlobStorageOptions.DefaultSection));

        services.AddTransient(serviceProvider => new BlobServiceClient(_configuration["AzureBlobStorage:ConnectionString"]));

        services.AddTransient<IFileStorageService, AzureBlobStorageService>();
    }
```