import { Routes } from '@angular/router';
import { Clients } from './clients/clients';
import { TrainingPlans } from './training-plans/training-plans';
import { NutritionPlans } from './nutrition-plans/nutrition-plans';
import { ptGuard } from '../../core/guards/pt-guard-guard';
import { Dashboard } from './pt-dashboard/pt-dashboard';

export const PT_ROUTES: Routes = [
    {
        path: 'dashboard',
        component: Dashboard,
        canActivate: [ptGuard]
    },
    {
        path: 'clients',
        component: Clients,
        canActivate: [ptGuard]
    },
    {
        path: 'training-plans',
        component: TrainingPlans,
        canActivate: [ptGuard]
    },
    {
        path: 'nutrition-plans',
        component: NutritionPlans,
        canActivate: [ptGuard]
    }
];