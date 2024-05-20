namespace Oauth2.sdk.Models
{
    public class AuthResponse
    {
        public string? Access_token { get; set; }
        public string? Refresh_token { get; set;}
        public string? Scope { get; set; }
        public int Expires_in { get; set; }
        public int Refresh_expires_in { get; set; }
    }
}
