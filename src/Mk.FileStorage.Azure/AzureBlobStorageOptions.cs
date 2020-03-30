namespace Mk.FileStorage.Azure
{
    public class AzureBlobStorageOptions
    {
        public const string DefaultSection = "AzureBlobStorage";

        public string? ContainerName { get; set; }
    }
}