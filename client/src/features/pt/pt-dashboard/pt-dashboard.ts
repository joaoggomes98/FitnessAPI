import { Component, inject, signal } from '@angular/core';
import { AuthService } from '../../../core/services/auth-service';
import { RouterLink } from '@angular/router';
import { PtService } from '../../../core/services/pt-service';
import { ToastService } from '../../../core/services/toast-service';

@Component({
  selector: 'app-pt-dashboard',
  imports: [RouterLink],
  templateUrl: './pt-dashboard.html',
  styleUrl: './pt-dashboard.css',
})
export class Dashboard {
  private ptService = inject(PtService);
  private authService = inject(AuthService);
  private toastService = inject(ToastService);

  user = this.authService.currentUser;

  totalClients = signal(0);
  loading = signal(false);

  ngOnInit(): void {
    this.loadStats();
  }

  loadStats() {
    this.loading.set(true);

    this.ptService.getClients().subscribe({
      next: clients => {
        this.totalClients.set(clients.length);
        this.loading.set(false);
      },
      error: err => {
        console.error(err);
        this.toastService.error('Não foi possível carregar os dados do dashboard.');
        this.loading.set(false);
      }
    });
  }
}


