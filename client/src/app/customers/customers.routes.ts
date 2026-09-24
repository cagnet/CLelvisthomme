import { Routes } from '@angular/router';

export const CUSTOMER_ROUTES: Routes = [
    {
        path: '',
        loadComponent: () => import('./customer-list-page/customer-list-page').then((m) => m.CustomerListPage)
    },
    {
        path: 'new',
        loadComponent: () => import('./customer-form-page/customer-form-page').then((m) => m.CustomerFormPage)
    },
    {
        path: ':id',
        loadComponent: () => import('./customer-detail-page/customer-detail-page').then((m) => m.CustomerDetailPage)
    },
    {
        path: ':id/edit',
        loadComponent: () => import('./customer-form-page/customer-form-page').then((m) => m.CustomerFormPage)
    }
];
