import { Routes } from '@angular/router';
import { TrainingPlanPage } from './training-plan/training-plan';
import { NutritionPlanPage } from './nutrition-plan/nutrition-plan';
import { clientGuard } from '../../core/guards/client-guard';
import { ClientDasboardPage } from './dasboard/dasboard';

export const CLIENT_ROUTES: Routes = [
    {
        path: 'dashboard',
        component: ClientDasboardPage,
        canActivate: [clientGuard]
    },
    {
        path: 'training-plan',
        component: TrainingPlanPage,
        canActivate: [clientGuard]
    },
    {
        path: 'nutrition-plan',
        component: NutritionPlanPage,
        canActivate: [clientGuard]
    }
];