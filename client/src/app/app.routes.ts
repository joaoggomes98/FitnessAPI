import { Routes } from '@angular/router';
import { PublicLayout } from '../layout/public-layout/public-layout';
import { Home } from '../features/home/home';
import { PrivateLayout } from '../layout/private-layout/private-layout';


export const routes: Routes = [
  {
    path: '',
    component: PublicLayout,
    children: [
      { path: '', component: Home },
      {
        path: 'auth',
        loadChildren: () =>
          import('../features/auth/auth.routes').then(m => m.AUTH_ROUTES)
      }
    ]
  },
  {
    path: '',
    component: PrivateLayout,
    children: [
      {
        path: 'super-admin',
        loadChildren: () =>
          import('../features/super-admin/super-admin.routes').then(m => m.SUPER_ADMIN_ROUTES)
      },
      {
        path: 'pt',
        loadChildren: () =>
          import('../features/pt/pt.routes').then(m => m.PT_ROUTES)
      },
      {
        path: 'client',
        loadChildren: () =>
          import('../features/client/client.routes').then(m => m.CLIENT_ROUTES)
      }
    ]
  }
];