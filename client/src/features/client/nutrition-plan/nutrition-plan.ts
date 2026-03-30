import { Component, inject, OnInit, signal } from '@angular/core';
import { NutritionService } from '../../../core/services/nutrition-service';
import { NutritionPlan } from '../../../core/models/nutrition-Plans/nutrition-plan';

@Component({
  selector: 'app-nutrition-plan',
  imports: [],
  templateUrl: './nutrition-plan.html',
  styleUrl: './nutrition-plan.css',
})
export class NutritionPlanPage implements OnInit{

  private nutritionService = inject(NutritionService)

  plans = signal<NutritionPlan[]>([])
  loading = signal(false)
  error = signal('')

  ngOnInit(): void {
    this.loadPlans()
  }

  loadPlans(){

    this.loading.set(true)

    this.nutritionService.getNutritionPlan().subscribe({

      next: response => {
        this.plans.set(response)
        this.loading.set(false)
      },

      error: err => {
        console.error(err)
        this.error.set('Não foi possível carregar os planos.')
        this.loading.set(false)
      }

    })

  }

}
