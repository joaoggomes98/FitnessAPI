using AutoMapper;
using API.Models;
using API.DTOs;


public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, UserDto>()
    .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Name));
            

        // AppUser -> MemberDto
        CreateMap<AppUser, MemberDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email ?? string.Empty))
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Name));

            CreateMap<TrainingPlan, TrainingPlanDto>();
            CreateMap<NutritionPlan, NutritionPlanDto>();
    }
}