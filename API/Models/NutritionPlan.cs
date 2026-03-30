namespace API.Models;

/// <summary>
    /// Representa um plano de nutrição atribuído a um cliente e criado por um PT.
    /// </summary>
    public class NutritionPlan
    {
        /// <summary>
        /// Identificador único do plano.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Título do plano de nutrição.
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Descrição detalhada do plano.
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// Id do cliente dono do plano.
        /// </summary>
        public string ClientId { get; set; } = null!;

        /// <summary>
        /// Cliente dono do plano.
        /// </summary>
        public AppUser User { get; set; } = null!;

        /// <summary>
        /// Id do PT que criou o plano.
        /// </summary>
        public string CreatedByPtId { get; set; } = null!;

        /// <summary>
        /// Personal Trainer que criou o plano.
        /// </summary>
        public AppUser CreatedByPt { get; set; } = null!;

        /// <summary>
        /// Data de criação do plano.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }