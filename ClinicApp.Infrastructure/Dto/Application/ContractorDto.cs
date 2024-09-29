namespace ClinicApp.Infrastructure.Dtos.Application
{
    public class CreateContractorRequest
    {
        public string? Name { get; set; }
        public string? RenderingProvider { get; set; }
        public bool Enabled { get; set; }
        public string? Extra { get; set; }
        public List<CreatePayrollRequest>? Payrolls { get; set; }
    }
    public class CreateContractorResponse
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string? RenderingProvider { get; set; } = string.Empty;
        public bool Enabled { get; set; } = false;
        public string? Extra { get; set; } = string.Empty;
    }

    public class UpdateContractorRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? RenderingProvider { get; set; } = string.Empty;
        public bool Enabled { get; set; } = false;
        public string? Extra { get; set; } = string.Empty;
        public List<CreatePayrollRequest>? Payrolls { get; set; }
    }
    public class UpdateContractorResponse
    {
        public int Id { get; set; }
    }

    public class GetAllContractorsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? RenderingProvider { get; set; }
        public bool Enabled { get; set; }
        public string? Extra { get; set; }
    }

    public class GetContractorByIdResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? RenderingProvider { get; set; }
        public bool Enabled { get; set; }
        public string? Extra { get; set; }
        public List<GetContractorByIdResponse>? Payrolls { get; set; }
    }
}
