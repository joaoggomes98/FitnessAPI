using Microsoft.AspNetCore.Identity;

namespace API.Models
{
    public class AppUser : IdentityUser
    {
        public required string Name { get; set; }

        /*
        =========================
        RELAÇÃO PT → CLIENT
        =========================
        */

        public string? TrainerId { get; set; }

        public AppUser? Trainer { get; set; }

        public ICollection<AppUser> Clients { get; set; } = new List<AppUser>();


        /*
        =========================
        RELAÇÃO SUPERADMIN → PT
        =========================
        */

        public string? SuperAdminId { get; set; }

        public AppUser? SuperAdmin { get; set; }

        public ICollection<AppUser> PTs { get; set; } = new List<AppUser>();


        /*
        =========================
        PLANOS DO CLIENT
        =========================
        */

        public ICollection<TrainingPlan> ClientTrainingPlans { get; set; } = new List<TrainingPlan>();

        public ICollection<NutritionPlan> ClientNutritionPlans { get; set; } = new List<NutritionPlan>();


        /*
        =========================
        PLANOS CRIADOS PELO PT
        =========================
        */

        public ICollection<TrainingPlan> CreatedTrainingPlans { get; set; } = new List<TrainingPlan>();

        public ICollection<NutritionPlan> CreatedNutritionPlans { get; set; } = new List<NutritionPlan>();
    }
}