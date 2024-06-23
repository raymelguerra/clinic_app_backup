
using AutoMapper;

namespace ClinicApp.Api.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Oauth2.sdk.Models.User, Core.Dtos.UserVM>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.SurName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Enabled, opt => opt.MapFrom(src => src.Enabled))
            .ReverseMap()
            .ForMember(dest => dest.RequiredActions, opt => opt.MapFrom(
                src => new List<string> { "UPDATE_PASSWORD" }));
        }
    }
}
