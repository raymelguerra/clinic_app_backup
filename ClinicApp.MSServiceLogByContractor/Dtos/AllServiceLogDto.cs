using ClinicApp.Core.Models;
namespace ClinicApp.MSServiceLogByContractor.Dtos;

public class AllServiceLogDto
{
    public int ServiceLogId { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.MinValue;
    public string PeriodRange { get; set; }= string.Empty;
    public string ClientName { get; set; } = string.Empty ;
    public ServiceLogStatus ServiceLogStatus { get; set; } = ServiceLogStatus.CREATED;
}
