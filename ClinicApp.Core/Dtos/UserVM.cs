using ClinicApp.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace ClinicApp.Core.Dtos;

public class UserVM
{
    public string? Id { get; set; }
    [Required(ErrorMessage = "Username is required")]
    public string? Username { get; set; }

    public bool Enabled { get; set; } = true;

    [Required(ErrorMessage = "Name is required")]
    [NotNullOrOnlyNumbers(ErrorMessage = "Invalid Name")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Surname is required")]
    [NotNullOrOnlyNumbers(ErrorMessage = "Invalid Surname")]
    public string? SurName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email address")]
    public string? Email { get; set; }

    public string? Department { get; set; }

    public IEnumerable<Oauth2.sdk.Models.Role>? Roles { get; set; }

    public IEnumerable<ValidationResult> Validate()
    {
        var results = new List<ValidationResult>();

        Validator.TryValidateProperty(Username, new ValidationContext(this, null, null) { MemberName = nameof(Username) }, results);
        Validator.TryValidateProperty(Name, new ValidationContext(this, null, null) { MemberName = nameof(Name) }, results);
        Validator.TryValidateProperty(SurName, new ValidationContext(this, null, null) { MemberName = nameof(SurName) }, results);
        Validator.TryValidateProperty(Email, new ValidationContext(this, null, null) { MemberName = nameof(Email) }, results);

        return results;
    }
}
