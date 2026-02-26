import { inject } from '@angular/core';
import { Routes } from '@angular/router';
import { Common } from '../../services/common';

const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./protection-packages'),
    canActivate:[()=>inject(Common).checkPermissionForRouting('protection:view')]
  },
  {
    path: 'add',
    loadComponent:()=>import('./create/create'),
    canActivate:[()=>inject(Common).checkPermissionForRouting('protection:create')]
  },
  {
    path: 'edit/:id',
    loadComponent: () => import('./create/create'),
    canActivate:[()=>inject(Common).checkPermissionForRouting('protection:edit')]
  },
  {
    path: 'details/:id',
    loadComponent: () => import('./detail/detail'),
    canActivate:[()=>inject(Common).checkPermissionForRouting('protection:view')]
  },
];

export default routes;
