import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { PtService } from '../../../core/services/pt-service';
import { Client } from '../../../core/models/users/client';
import { CreateClient } from '../../../core/models/users/create-client';
import { UpdateClient } from '../../../core/models/users/update-client';
import { ToastService } from '../../../core/services/toast-service';

@Component({
  selector: 'app-pt-clients',
  imports: [FormsModule],
  templateUrl: './clients.html',
  styleUrl: './clients.css'
})
export class Clients implements OnInit {
  private ptService = inject(PtService);
  private toastService = inject(ToastService);
  
  clients = signal<Client[]>([]);
  loading = signal(false);

  model: CreateClient = {
    name: '',
    email: '',
    password: ''
  };

  editingClientId = signal<string | null>(null);

  editModel: UpdateClient = {
    name: '',
    email: ''
  };
  ngOnInit(): void {
    this.loadClients();
  }

  loadClients() {
    this.loading.set(true);

    this.ptService.getClients().subscribe({
      next: response => {
        this.clients.set(response);
        this.loading.set(false);
      },
      error: err => {
        console.error(err);
        this.toastService.error('Não foi possível carregar os clientes.');
        this.loading.set(false);
      }
    });
  }

  createClient() {

    this.ptService.createClient(this.model).subscribe({
      next: () => {
        this.toastService.success('Cliente criado com sucesso.');

        this.model = {
          name: '',
          email: '',
          password: ''
        };

        this.loadClients();
      },
      error: err => {
        console.error(err);
        this.toastService.error('Não foi possível criar o cliente.');
      }
    });
  }

  startEdit(client: Client) {
    this.editingClientId.set(client.id);
    this.editModel = {
      name: client.name,
      email: client.email
    };
  }

  cancelEdit() {
    this.editingClientId.set(null);
    this.editModel = {
      name: '',
      email: ''
    };
  }

  saveEdit(id: string) {

    this.ptService.updateClient(id, this.editModel).subscribe({
      next: () => {
        this.toastService.success('Cliente atualizado com sucesso.');
        this.cancelEdit();
        this.loadClients();
      },
      error: err => {
        console.error(err);
        this.toastService.error('Não foi possível atualizar o cliente.');
      }
    });
  }

  deleteClient(id: string) {

    this.ptService.deleteClient(id).subscribe({
      next: () => {
        this.toastService.success('Cliente removido com sucesso.');
        this.loadClients();
      },
      error: err => {
        console.error(err);
        this.toastService.error('Não foi possível remover o cliente.');
      }
    });
  }
}