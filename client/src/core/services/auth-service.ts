import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { Login } from '../models/auth/login';
import { CurrentUser } from '../models/auth/current-user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private baseUrl = 'https://localhost:5001/api/';

  currentUser = signal<CurrentUser | null>(null);

  login(credentials: Login) {
    return this.http.post<CurrentUser>(`${this.baseUrl}auth/login`, credentials).pipe(
      tap(user => {
        localStorage.setItem('token', user.token);
        localStorage.setItem('user', JSON.stringify(user));
        this.currentUser.set(user);
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.currentUser.set(null);
  }

  loadCurrentUser() {
    const user = localStorage.getItem('user');
    if (user) {
      this.currentUser.set(JSON.parse(user));
    }
  }

  getToken() {
    return localStorage.getItem('token');
  }

  isLoggedIn() {
    return !!this.getToken();
  }

  getRole() {
    return this.currentUser()?.role ?? null;
  }
}