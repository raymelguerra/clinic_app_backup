
namespace ClinicApp.Infrastructure.Dtos.Application;

public class GetCompanyByIdResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Acronym { get; set; } = null!;
    public bool Enabled { get; set; } = true;
}
