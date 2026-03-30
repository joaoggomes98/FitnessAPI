using System;

namespace API.DTOs;

public class UpdateTrainingPlanDto
{
    public string? Title { get; set; }  // Novo título do plano de treino (opcional)
    public string? Description { get; set; }  // Nova descrição do plano de treino (opcional)
}