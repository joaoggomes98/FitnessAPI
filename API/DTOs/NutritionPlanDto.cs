using System;

namespace API.DTOs;

public class NutritionPlanDto
{
    public int Id { get; set; }  // ID do plano de nutrição
    public string Title { get; set; }  // Título do plano de nutrição
    public string Description { get; set; }  // Descrição do plano de nutrição
    public string ClientId { get; set; }  // ID do cliente associado
    public string CreatedByPtId { get; set; }  // ID do Personal Trainer que criou o plano
}
