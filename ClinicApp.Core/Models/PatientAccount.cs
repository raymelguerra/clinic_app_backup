using System.ComponentModel.DataAnnotations;

namespace ClinicApp.Core.Models;

public class PatientAccount
{
    [Key]
    public int Id { get; set; }

    public string LicenseNumber { get; set; } = string.Empty;

    public string? Auxiliar { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? ExpireDate { get; set; }

}
