using System;

namespace API.DTOs;

public class UpdateNutritionPlanDto
{
    public string? Title { get; set; }  // Novo título do plano de nutrição (opcional)
    public string? Description { get; set; }  // Nova descrição do plano de nutrição (opcional)
}
