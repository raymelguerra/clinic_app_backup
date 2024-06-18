
using System.ComponentModel.DataAnnotations;

namespace ClinicApp.Core.Models;

public class Insurance
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public DateTime? EntryDate { get; set; }

    [Required]
    public DateTime? ExpirationDate { get; set; }

    public string? Logo { get; set; }

    public virtual List<InsuranceProcedure>? InsuranceProcedures { get; set; }

}
