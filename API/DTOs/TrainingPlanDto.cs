using System;

namespace API.DTOs;

public class TrainingPlanDto
{
    public int Id { get; set; }  // ID do plano de treino
    public string Title { get; set; }  // Título do plano de treino
    public string Description { get; set; }  // Descrição do plano de treino
    public string ClientId { get; set; }  // ID do cliente associado
    public string CreatedByPtId { get; set; }  // ID do Personal Trainer que criou o plano
}
