using System.ComponentModel.DataAnnotations;

namespace ClinicApp.Core.Models;

public class Period
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    public bool Active { get; set; }

    [Required]
    public string PayPeriod { get; set; } = string.Empty;

    [Required]
    public DateTime DocumentDeliveryDate { get; set; }

    [Required]
    public DateTime PaymentDate { get; set; }
}