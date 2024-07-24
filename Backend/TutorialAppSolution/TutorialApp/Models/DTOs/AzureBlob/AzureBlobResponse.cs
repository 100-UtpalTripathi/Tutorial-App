namespace TutorialApp.Models.DTOs.AzureBlob
{
    public class AzureBlobResponse
    {
        public string FileName { get; set; }
        public Uri FileUri { get; set; }

        public int StatusCode { get; set; }

        public bool IsError { get; set; }
    }
}
