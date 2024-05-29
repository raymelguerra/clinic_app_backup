namespace Oauth2.sdk.Models
{
    public class CredentialsSettings
    {
        public string? Authority { get; set; }
        public string? Realm { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? GrantType { get; set; }
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
        public string? Scope { get; set; }
        public string? RedirectUri { get; set; }
        public string? Audience { get; set; } 
        public int? InactivityTime { get; set; }
        public bool GrandAccountSelfmanagament { get; set; } = false;
        public string? ClientApiId { get; set; }
    }
}
