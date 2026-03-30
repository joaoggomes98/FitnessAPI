import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { Login as LoginModel } from '../../../core/models/auth/login';
import { AuthService } from '../../../core/services/auth-service';

@Component({
  selector: 'app-login',
  imports: [FormsModule,RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  private authService = inject(AuthService);
  private router = inject(Router);

  creds: LoginModel = {
    email: '',
    password: ''
  };

  login() {
    this.authService.login(this.creds).subscribe({
      next: user => {
        if (user.role === 'SuperAdmin') {
          this.router.navigateByUrl('/super-admin/dashboard');
        } else if (user.role === 'PT') {
          this.router.navigateByUrl('/pt/dashboard');
        } else {
          this.router.navigateByUrl('/client/dashboard');
        }
      },
      error: err => {
        console.error('Erro no login', err);
      }
    });
  }
}