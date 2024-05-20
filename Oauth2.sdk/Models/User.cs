namespace Oauth2.sdk.Models
{
    public class User
    {
        public string Id { get; set; }
        public long CreatedTimestamp { get; set; }
        public string Username { get; set; }
        public bool Enabled { get; set; } = true;
        public bool Totp { get; set; }
        public bool EmailVerified { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public Dictionary<string, List<string>> Attributes { get; set; }
        public List<string>? DisableableCredentialTypes { get; set; }
        public List<string>? RequiredActions { get; set; }
        public int NotBefore { get; set; }
        public Access? Access { get; set; }
        public IEnumerable<string>? Roles { get; set; }
    }

    public class Access
    {
        public bool ManageGroupMembership { get; set; }
        public bool View { get; set; }
        public bool MapRoles { get; set; }
        public bool Impersonate { get; set; }
        public bool Manage { get; set; }
    }
}