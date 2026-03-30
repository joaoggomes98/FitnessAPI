using API.Interfaces;
using API.Services;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ITrainingPlanService, TrainingPlanService>();
        services.AddScoped<INutritionPlanService, NutritionPlanService>();

        services.AddAutoMapper(typeof(AutoMapperProfiles));

        return services;
    }
}