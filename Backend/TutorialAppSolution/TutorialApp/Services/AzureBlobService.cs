using Azure;
using Azure.Storage.Blobs;
using TutorialApp.Interfaces;
using TutorialApp.Models.DTOs.AzureBlob;

namespace TutorialApp.Services
{
    public class AzureBlobService : IAzureBlobService
    {

        #region Dependency Injection
        private readonly BlobServiceClient _blobServiceClient;
        public AzureBlobService(BlobServiceClient blobServiceClient) {
            _blobServiceClient = blobServiceClient;
        }

        #endregion


        #region Upload File
        public async Task<AzureBlobResponse> UploadFileAsync(string blobContainer, string directoryName, string fileName, Stream fileStream)
        {
            var BlobResponse = new AzureBlobResponse();
            try
            {
                // Create a BlobServiceClient object which will be used to create a container client
                var container = _blobServiceClient.GetBlobContainerClient(blobContainer);
                await container.CreateIfNotExistsAsync();


                // Get a reference to a blob
                var blob = container.GetBlobClient(directoryName + "/" + fileName);

                // Upload data from a stream
                var blobResult = await blob.UploadAsync(fileStream);

                //var decodedUri = Uri.UnescapeDataString(blob.Uri.ToString());

                // Return the response
                BlobResponse =  new AzureBlobResponse
                {
                    FileName = blob.Name,
                    FileUri = blob.Uri,
                    StatusCode = (int)blobResult.GetRawResponse().Status,
                    IsError = false
                };
            }
            catch(RequestFailedException rfex)
            {
                BlobResponse = new AzureBlobResponse
                {
                    FileName = fileName,
                    FileUri = null,
                    StatusCode = (int)rfex.Status,
                    IsError = true
                };
            }
            catch (Exception ex)
            {
                BlobResponse = new AzureBlobResponse
                {
                    FileName = fileName,
                    FileUri = null,
                    StatusCode = 500,
                    IsError = true
                };
            }
            
            return BlobResponse;
            
        }

        #endregion
    }
}
