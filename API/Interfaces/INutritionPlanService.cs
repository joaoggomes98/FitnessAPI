using System;
using API.DTOs;

namespace API.Interfaces;

public interface INutritionPlanService
{
    Task<NutritionPlanDto> CreateNutritionPlan(string ptId,string clientId, CreateNutritionPlanDto dto);

    Task<NutritionPlanDto?> UpdateNutritionPlan(string ptId, int planId, UpdateNutritionPlanDto dto);

    Task<bool> DeleteNutritionPlan(string ptId, int planId);
    Task<IEnumerable<NutritionPlanDto>> GetAllNutritionPlans(string ptId, string clientId);
    Task<IEnumerable<NutritionPlanDto>> GetNutritionPlanForClient(string clientId);
}
