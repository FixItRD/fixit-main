namespace fixit_main.Models
{
    public class RegisterResponse
    {
        public RegisterResponse(int statusCode, string message = null, string token = null)
        {
            Message = message;
            Token = token;
            StatusCode = statusCode;
        }

        public string Message { get; set; }
        public string Token { get; set; }
        public int StatusCode { get; set; }
    }
}
