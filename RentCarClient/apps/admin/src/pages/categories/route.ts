import { inject } from '@angular/core';
import { Routes } from '@angular/router';
import { Common } from '../../services/common';

const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./categories'),
    canActivate:[()=>inject(Common).checkPermissionForRouting('category:view')]
  },
  {
    path: 'add',
    loadComponent: () => import('./create/create'),
    canActivate:[()=>inject(Common).checkPermissionForRouting('category:create')]
  },
  {
    path: 'edit/:id',
    loadComponent: () => import('./create/create'),
    canActivate:[()=>inject(Common).checkPermissionForRouting('category:edit')]
  },
  {
    path: 'details/:id',
    loadComponent: () => import('./detail/detail'),
    canActivate:[()=>inject(Common).checkPermissionForRouting('category:view')]
  },
];

export default routes;
