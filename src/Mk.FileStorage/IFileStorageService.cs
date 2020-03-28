using System.IO;
using System.Threading.Tasks;

namespace Mk.FileStorage
{
    public interface IFileStorageService
    {
        Task UploadAsync(string fileName, Stream stream);

        Task DownloadToAsync(string fileName, Stream stream);
    }
}