namespace Mk.FileStorage.Azure
{
    public class AzureBlobStorageOptions
    {
        public const string DefaultSection = "AzureBlobStorage";

        public string? ConnectionString { get; set; }

        public string? ContainerName { get; set; }
    }
}