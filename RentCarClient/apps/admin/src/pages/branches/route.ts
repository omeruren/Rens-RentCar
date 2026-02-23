import { inject } from '@angular/core';
import { Routes } from '@angular/router';
import { Common } from '../../services/common';

const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./branches'),
    canActivate:[()=>inject(Common).checkPermissionForRouting('branch:view')]
  },
  {
    path: 'add',
    loadComponent: () => import('./create/create'),
    canActivate:[()=>inject(Common).checkPermissionForRouting('branch:create')]
  },
  {
    path: 'edit/:id',
    loadComponent: () => import('./create/create'),
    canActivate:[()=>inject(Common).checkPermissionForRouting('branch:edit')]
  },
  {
    path: 'details/:id',
    loadComponent: () => import('./detail/detail'),
    canActivate:[()=>inject(Common).checkPermissionForRouting('branch:view')]
  },
];

export default routes;
