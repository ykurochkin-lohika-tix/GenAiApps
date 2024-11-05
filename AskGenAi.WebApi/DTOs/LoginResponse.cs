namespace AskGenAi.WebApi.DTOs
{
    public class LoginResponse(string token)
    {
        public string Token { get; set; } = token;
    }
}
