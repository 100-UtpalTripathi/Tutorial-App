using TutorialApp.Models.DTOs.AzureBlob;

namespace TutorialApp.Interfaces
{
    public interface IAzureBlobService
    {
        Task<AzureBlobResponse> UploadFileAsync(string blobContainer, string directoryName, 
            string fileName, Stream fileStream);
    }
}
