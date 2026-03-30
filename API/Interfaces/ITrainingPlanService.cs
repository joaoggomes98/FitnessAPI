using System;
using API.DTOs;

namespace API.Interfaces;

public interface ITrainingPlanService
{
Task<IEnumerable<TrainingPlanDto>> GetTrainingPlanForClient(string clientId);
Task<IEnumerable<TrainingPlanDto>> GetAllTrainingPlans(string ptId, string clientId);
Task<TrainingPlanDto> CreateTrainingPlan(string ptId, string clientId, CreateTrainingPlanDto dto);
Task<TrainingPlanDto> UpdateTrainingPlan(string ptId, string clientId, int planId, UpdateTrainingPlanDto dto);
Task<bool> DeleteTrainingPlan(string ptId, string clientId, int planId);
}
