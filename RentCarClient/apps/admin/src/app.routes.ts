import { Route } from '@angular/router';
import { authGuard } from './guards/auth-guard';

export const appRoutes: Route[] = [
  {
    path: '',
    loadComponent: () => import('./pages/layouts/layouts'),
    canActivateChild: [authGuard],
    children: [
      {
        path: '',
        loadComponent: () => import('./pages/dashboard/dashboard'),
      },
    ],
  },
  {
    path: 'login',
    loadComponent: () => import('./pages/login/login'),
  },
];
