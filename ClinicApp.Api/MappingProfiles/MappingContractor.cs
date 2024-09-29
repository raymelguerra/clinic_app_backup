using AutoMapper;
using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Dtos.Application;

namespace ClinicApp.Api.MappingProfiles
{
    public class MappingContractor : Profile
    {
        public MappingContractor()
        {
            CreateMap<CreateContractorRequest, Contractor>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.RenderingProvider, opt => opt.MapFrom(src => src.RenderingProvider))
                .ForMember(dest => dest.Enabled, opt => opt.MapFrom(src => src.Enabled))
                .ForMember(dest => dest.Extra, opt => opt.MapFrom(src => src.Extra))
                .ForMember(dest => dest.Payrolls, opt => opt.MapFrom(src => src.Payrolls));

            CreateMap<Contractor, CreateContractorResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.RenderingProvider, opt => opt.MapFrom(src => src.RenderingProvider))
                .ForMember(dest => dest.Enabled, opt => opt.MapFrom(src => src.Enabled))
                .ForMember(dest => dest.Extra, opt => opt.MapFrom(src => src.Extra));

            CreateMap<Contractor, GetAllContractorsResponse>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.RenderingProvider, opt => opt.MapFrom(src => src.RenderingProvider))
               .ForMember(dest => dest.Enabled, opt => opt.MapFrom(src => src.Enabled))
               .ForMember(dest => dest.Extra, opt => opt.MapFrom(src => src.Extra));

            CreateMap<Contractor, GetContractorByIdResponse>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.RenderingProvider, opt => opt.MapFrom(src => src.RenderingProvider))
               .ForMember(dest => dest.Enabled, opt => opt.MapFrom(src => src.Enabled))
               .ForMember(dest => dest.Extra, opt => opt.MapFrom(src => src.Extra));
        }
    }
}
