namespace Register.Microservice.Models
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string access_token { get; set; } = string.Empty;
        public int expires_in { get; set; }
    }
}
