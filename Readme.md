# Mk.FileStorage

Mk.FileStorage is a simple solution to integrate a File Storage in your ASP.NET Core project using streaming.

![CI](https://github.com/m-knet/Mk.FileStorage/workflows/CI/badge.svg)
![Continuous Deployment PREVIEW](https://github.com/m-knet/Mk.FileStorage/workflows/Continuous%20Deployment%20PREVIEW/badge.svg)

## Getting Started

### Prerequisites

* .NET Core 3.1 / 5.0
* Docker (optional, to mock Azure Blob Storage with Azurite)

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
            endpoints.MapMkFileStorage();
        });
    }
```

The default endpoint is /files, but you can choice other setting pattern:

```csharp
    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapMkFileStorage(pattern: "otherendpoint");
        });
    }
```

You can register services calling `AddAzureMkFileStorage` with configuration and connection string:

```csharp
    public void ConfigureServices(IServiceCollection services)
    {
        var connectionString = _configuration.GetConnectionString("AzureBlobStorage");

        if (_env.IsDevelopment())
        {
            CreateContainerIfNotExists(connectionString);
        }

        services.AddAzureMkFileStorage(_configuration, connectionString);
    }
```

Configuration can be as follow in appsettings:
```json
    {
      "AzureBlobStorage": {
        "ContainerName": "default"
      }
    }
```

Run sample in Visual Studio or with command line:

```csharp
dotnet run --project src\Mk.FileStorage.Sample\Mk.FileStorage.Sample.csproj
```

And upload/download files from https://localhost:5001/files/{fileName}
 
