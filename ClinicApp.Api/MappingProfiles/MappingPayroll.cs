using AutoMapper;
using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Dtos.Application;

namespace ClinicApp.Api.MappingProfiles
{
    public class PayrollContractor : Profile
    {
        public PayrollContractor()
        {
            // Mapea de CreateContractorRequest a Contractor
            CreateMap<CreatePayrollRequest, Payroll>()
                .ForMember(dest => dest.InsuranceProcedureId, opt => opt.MapFrom(src => src.InsuranceProcedureId))
                .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
                .ForMember(dest => dest.ContractorTypeId, opt => opt.MapFrom(src => src.ContractorTypeId));

        }
    }
}
