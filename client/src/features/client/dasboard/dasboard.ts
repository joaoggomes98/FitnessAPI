import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth-service';

@Component({
  selector: 'app-dasboard',
  imports: [RouterLink],
  templateUrl: './dasboard.html',
  styleUrl: './dasboard.css',
})
export class ClientDasboardPage {
private authService = inject(AuthService);

  user = this.authService.currentUser;
}
