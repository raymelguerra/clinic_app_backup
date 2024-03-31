using System.ComponentModel.DataAnnotations;

namespace ClinicApp.Core.Models;

public class Company
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public string Acronym { get; set; } = null!;

    public bool Enabled { get; set; } = true;
}
