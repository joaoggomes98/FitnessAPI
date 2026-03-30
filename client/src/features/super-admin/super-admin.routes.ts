import { Routes } from '@angular/router';
import { Pts } from './pts/pts';
import { SuperAdminDashboard } from './super-admin-dashboard/super-admin-dashboard';
import { superAdminGuard } from '../../core/guards/admin-guard';

export const SUPER_ADMIN_ROUTES: Routes = [
    {
        path: 'dashboard',
        component: SuperAdminDashboard,
        canActivate: [superAdminGuard]
    },
    {
        path: 'pts',
        component: Pts,
        canActivate: [superAdminGuard]
    }
];