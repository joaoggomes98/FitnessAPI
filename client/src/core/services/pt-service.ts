import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Client } from '../models/users/client';
import { CreateClient } from '../models/users/create-client';
import { UpdateClient } from '../models/users/update-client';

@Injectable({
  providedIn: 'root'
})
export class PtService {
  private http = inject(HttpClient);
  private baseUrl = 'https://localhost:5001/api/pt/';

  getClients() {
    return this.http.get<Client[]>(`${this.baseUrl}clients`);
  }

  getClientById(id: string) {
    return this.http.get<Client>(`${this.baseUrl}clients/${id}`);
  }

  createClient(model: CreateClient) {
    return this.http.post<Client>(`${this.baseUrl}clients`, model);
  }
  updateClient(id: string, model: UpdateClient){
    return this.http.put<Client>(`${this.baseUrl}clients/${id}`, model);
  }

  deleteClient(id: string) {
    return this.http.delete(`${this.baseUrl}clients/${id}`);
  }
}