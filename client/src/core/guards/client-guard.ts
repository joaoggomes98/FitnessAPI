import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth-service';

export const clientGuard: CanActivateFn = () => {

  const authService = inject(AuthService)
  const router = inject(Router)

  const user = authService.currentUser()

  if(user?.role === 'Client'){
    return true
  }

  router.navigateByUrl('/')
  return false
}
