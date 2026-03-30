import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Member } from '../models/users/member';
import { CreateUser } from '../models/users/create-user';
import { UpdateUser } from '../models/users/update-user';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private http = inject(HttpClient);
  private baseUrl = 'https://localhost:5001/api/admin/';

  getUsers(){
    return this.http.get<Member[]>(`${this.baseUrl}users`);
  }
  getUserById(id: string){
    return this.http.get<Member>(`${this.baseUrl}users/${id}`)
  }
  createUser(createUserAux: CreateUser){
    return this.http.post<Member>(`${this.baseUrl}users`, createUserAux);
  }
  updateUser(id: string, updateUserAux: UpdateUser){
    return this.http.put<Member>(`${this.baseUrl}users/${id}`, updateUserAux);
  }
  DeleteUser(id: string){
    return this.http.delete(`${this.baseUrl}users/${id}`);
  }
  GetRoles(){
  }
  createRole(){}
  updateRole(){}
  deleteRole(){}
  assignRole(){}
  removeRoleFromUser(){}
  getUserRoles(){}
  
}
