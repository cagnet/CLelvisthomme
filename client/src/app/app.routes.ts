import { Routes } from '@angular/router';

export const routes: Routes = [
    { path: '', redirectTo: 'customers', pathMatch: 'full' },
    {
        path: 'customers',
        loadChildren: () => import('./customers/customers.routes').then((m) => m.CUSTOMER_ROUTES)
    },
    {
        path: 'dashboard',
        loadComponent: () => import('./dashboard/dashboard-page/dashboard-page').then((m) => m.DashboardPage)
    },
    { path: '**', redirectTo: 'customers' },
];
