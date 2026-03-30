import { Component, inject, signal } from '@angular/core';
import { AuthService } from '../../../core/services/auth-service';
import { AdminService } from '../../../core/services/admin-service';

@Component({
  selector: 'app-super-admin-dashboard',
  imports: [],
  templateUrl: './super-admin-dashboard.html',
  styleUrl: './super-admin-dashboard.css',
})
export class SuperAdminDashboard {
  private authService = inject(AuthService);
  private adminService = inject(AdminService);

  user = this.authService.currentUser;

  totalPts = signal(0);
  loading = signal(false);
  error = signal('');

  ngOnInit(): void {
    this.loadStats();
  }

  loadStats() {
    this.loading.set(true);
    this.error.set('');

    this.adminService.getUsers().subscribe({
      next: users => {
        this.totalPts.set(users.length);
        this.loading.set(false);
      },
      error: err => {
        console.error(err);
        this.error.set('Não foi possível carregar os dados do dashboard.');
        this.loading.set(false);
      }
    });
  }
}
