import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AdminService } from '../../../core/services/admin-service';
import { Member } from '../../../core/models/users/member';
import { CreateUser } from '../../../core/models/users/create-user';
import { ToastService } from '../../../core/services/toast-service';

@Component({
  selector: 'app-pts',
  imports: [FormsModule],
  templateUrl: './pts.html',
  styleUrl: './pts.css'
})
export class Pts implements OnInit {
  private adminService = inject(AdminService);
  private toastService = inject(ToastService);

  users = signal<Member[]>([]);
  loading = signal(false);

  createModel: CreateUser = {
    name: '',
    email: '',
    password: '',
    role: 'PT'
  };

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers() {
    this.loading.set(true);

    this.adminService.getUsers().subscribe({
      next: response => {
        this.users.set(response);
        this.loading.set(false);
      },
      error: err => {
        console.error(err);
        this.toastService.error('Não foi possível carregar os utilizadores.');
        this.loading.set(false);
      }
    });
  }

  createPt() {
    this.adminService.createUser(this.createModel).subscribe({
      next: () => {
        this.toastService.success('PT criado com sucesso.');

        this.createModel = {
          name: '',
          email: '',
          password: '',
          role: 'PT'
        };

        this.loadUsers();
      },
      error: err => {
        console.error(err);
        this.toastService.error('Não foi possível criar o PT.');
      }
    });
  }

  deleteUser(id: string) {
    this.adminService.DeleteUser(id).subscribe({
      next: () => {
        this.toastService.success('PT removido com sucesso.');
        this.loadUsers();
      },
      error: err => {
        console.error(err);
        this.toastService.error('Não foi possível remover o PT.');
      }
    });
  }
}