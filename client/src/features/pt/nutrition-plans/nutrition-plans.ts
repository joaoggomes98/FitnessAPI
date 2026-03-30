import { Component, inject, signal } from '@angular/core';
import { PtService } from '../../../core/services/pt-service';
import { NutritionService } from '../../../core/services/nutrition-service';
import { CreateNutritionPlan } from '../../../core/models/nutrition-Plans/create-nutrition-plan';
import { Client } from '../../../core/models/users/client';
import { FormsModule } from '@angular/forms';
import { ToastService } from '../../../core/services/toast-service';

@Component({
  selector: 'app-nutrition-plans',
  imports: [FormsModule],
  templateUrl: './nutrition-plans.html',
  styleUrl: './nutrition-plans.css',
})
export class NutritionPlans {
  private ptService = inject(PtService);
  private nutritionService = inject(NutritionService);
  private toastService =  inject(ToastService);

  clients = signal<Client[]>([]);
  loadingClients = signal(false);

  model: CreateNutritionPlan = {
    title: '',
    description: '',
    clientId: ''
  };

  ngOnInit(): void {
    this.loadClients();
  }

  loadClients() {
    this.loadingClients.set(true);

    this.ptService.getClients().subscribe({
      next: response => {
        this.clients.set(response);
        this.loadingClients.set(false);
      },
      error: err => {
        console.error(err);
        this.toastService.success('Não foi possível carregar os clientes.');
        this.loadingClients.set(false);
      }
    });
  }

  createNutritionPlan() {

    this.nutritionService.createNutritionPlan(this.model).subscribe({
      next: () => {
        this.toastService.success('Plano de treino criado com sucesso.');

        this.model = {
          title: '',
          description: '',
          clientId: ''
        };
      },
      error: err => {
        console.error(err);
        this.toastService.error('Não foi possível criar o plano de treino.');
      }
    });
  }
}
