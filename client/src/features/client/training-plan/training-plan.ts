import { Component, inject, OnInit, signal } from '@angular/core';
import { TrainingService } from '../../../core/services/training-service';
import { TrainingPlan } from '../../../core/models/training-plans/training-plan';

@Component({
  selector: 'app-training-plan',
  imports: [],
  templateUrl: './training-plan.html',
  styleUrl: './training-plan.css',
})
export class TrainingPlanPage implements OnInit{

  private trainingService = inject(TrainingService)

  plans = signal<TrainingPlan[]>([])  
  loading = signal(false)
  error = signal('')

  ngOnInit(): void {
    this.loadPlan()
  }

  loadPlan(){

    this.loading.set(true)

    this.trainingService.getTrainingPlan().subscribe({

      next: response => {
        this.plans.set(response)
        this.loading.set(false)
      },

      error: err => {
        console.error(err)
        this.error.set('Não foi possível carregar o plano.')
        this.loading.set(false)
      }

    })

  }
}
