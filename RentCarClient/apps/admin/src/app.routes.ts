import { Route } from '@angular/router';
import { authGuard } from './guards/auth-guard';
import { inject } from '@angular/core';
import { Common } from './services/common';

export const appRoutes: Route[] = [
  {
    path: 'unauthorize',
    loadComponent: () => import('./pages/unauthorize/unauthorize'),
  },
  {
    path: 'login',
    loadComponent: () => import('./pages/auth/login/login'),
  },
  {
    path: 'reset-password/:id',
    loadComponent: () => import('./pages/auth/reset-password/reset-password'),
  },
  {
    path: '',
    loadComponent: () => import('./pages/layouts/layouts'),
    canActivateChild: [authGuard],
    children: [
      {
        path: '',
        loadComponent: () => import('./pages/dashboard/dashboard'),
        canActivate: [
          () => inject(Common).checkPermissionForRouting('dashboard:view'),
        ],
      },
      {
        path: 'branches',
        loadChildren: () => import('./pages/branches/route'),
      },
      {
        path: 'categories',
        loadChildren: () => import('./pages/categories/route'),
      },
      {
        path: 'extras',
        loadChildren: () => import('./pages/extras/route'),
      },
      {
        path: 'protection-packages',
        loadChildren: () => import('./pages/protection-packages/route'),
      },
      {
        path: 'roles',
        loadChildren: () => import('./pages/roles/route'),
      },
      {
        path: 'users',
        loadChildren: () => import('./pages/users/route'),
      },
      {
        path: 'vehicles',
        loadChildren: () => import('./pages/vehicles/route'),
      },
    ],
  },
];
