using System.ComponentModel.DataAnnotations;

namespace ClinicApp.Core.Models;

public partial class Role
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Role name is required")]
    [StringLength(100, ErrorMessage = "Role name can't be longer than 100 characters")]
    public string Role1 { get; set; } = string.Empty;

    public string? Description { get; set; }

    public void CopyFrom(Role role)
    {
        Role1 = role.Role1;
        Description = role.Description;
    }

    public IEnumerable<ValidationResult> Validate(IEnumerable<Role>? roles = null)
    {
        var results = new List<ValidationResult>();

        Validator.TryValidateProperty(Role1, new ValidationContext(this, null, null) { MemberName = nameof(Role1) }, results);


        if (roles != null && roles.Any(x => x.Role1 == this.Role1))
        {
            results.Add(new ValidationResult("Role already exists", new[] { nameof(Role1) }));
        }

        return results;
    }
}
