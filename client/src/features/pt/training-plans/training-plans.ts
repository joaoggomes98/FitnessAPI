import { Component, inject, OnInit, signal } from '@angular/core';
import { TrainingService } from '../../../core/services/training-service';
import { PtService } from '../../../core/services/pt-service';
import { Client } from '../../../core/models/users/client';
import { CreateTrainingPlan } from '../../../core/models/training-plans/create-training-plan';
import { FormsModule } from '@angular/forms';
import { ToastService } from '../../../core/services/toast-service';

@Component({
  selector: 'app-training-plans',
  imports: [FormsModule],
  templateUrl: './training-plans.html',
  styleUrl: './training-plans.css',
})
export class TrainingPlans implements OnInit {
  private ptService = inject(PtService);
  private trainingService = inject(TrainingService);
  private toastService = inject(ToastService); 

  clients = signal<Client[]>([]);
  loadingClients = signal(false);

  model: CreateTrainingPlan = {
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
        this.toastService.error('Não foi possível carregar os clientes.');
        this.loadingClients.set(false);
      }
    });
  }

  createTrainingPlan() {

    this.trainingService.createTrainingPlan(this.model).subscribe({
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
