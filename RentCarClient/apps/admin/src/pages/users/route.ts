import { inject } from '@angular/core';
import { Routes } from '@angular/router';
import { Common } from '../../services/common';

const router: Routes = [
  {
    path: '',
    loadComponent: () => import('./users'),
    canActivate: [() => inject(Common).checkPermissionForRouting('user:view')],
  },
  {
    path: 'add',
    loadComponent: () => import('./create/create'),
    canActivate: [
      () => inject(Common).checkPermissionForRouting('user:create'),
    ],
  },
  {
    path: 'edit/:id',
    loadComponent: () => import('./create/create'),
    canActivate: [
      () => inject(Common).checkPermissionForRouting('user:edit'),
    ],
  },
];
export default router;
