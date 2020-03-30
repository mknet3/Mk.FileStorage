using System;
using System.IO;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;

namespace Mk.FileStorage.Azure
{
    public class AzureBlobStorageService : IFileStorageService
    {
        private readonly BlobServiceClient _client;
        private readonly AzureBlobStorageOptions _options;

        public AzureBlobStorageService(IOptions<AzureBlobStorageOptions> options, BlobServiceClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task UploadAsync(string fileName, Stream stream)
        {
            var blob = GetBlobClient(fileName);
            await blob.UploadAsync(stream);
        }

        public async Task DownloadToAsync(string fileName, Stream stream)
        {
            try
            {
                var blob = GetBlobClient(fileName);
                await blob.DownloadToAsync(stream);
            }
            catch (RequestFailedException ex)
            {
                if (ex.ErrorCode == BlobErrorCode.BlobNotFound || ex.ErrorCode == BlobErrorCode.ContainerNotFound)
                {
                    throw new FileStorageException("File not found in azure storage", FileStorageErrorCode.FileNotFound, ex);
                }
            }
        }

        private BlobClient GetBlobClient(string fileName)
        {
            return _client
                .GetBlobContainerClient(_options.ContainerName)
                .GetBlobClient(fileName);
        }
    }
}