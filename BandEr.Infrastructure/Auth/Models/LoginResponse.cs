namespace BandEr.Infrastructure.Auth.Models
{
    public class LoginResponse
    {
        public AccessToken AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}