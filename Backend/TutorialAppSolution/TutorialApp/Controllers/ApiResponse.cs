namespace TutorialApp.Controllers
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public T Data { get; set; }

        public ApiResponse(int statusCode, string statusMessage, T data)
        {
            StatusCode = statusCode;
            StatusMessage = statusMessage;
            Data = data;
        }
    }

}
