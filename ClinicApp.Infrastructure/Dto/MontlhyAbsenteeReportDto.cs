
namespace ClinicApp.Infrastructure.Dto;

public class MontlhyAbsenteeReportDto
{
    public required string Month { get; set; }
    public required string PhysicianName { get; set; }
    public int AttendanceCount { get; set; }
    public string? RenderingProvider { get; set; }
    public string? Extra { get; set; }
}
