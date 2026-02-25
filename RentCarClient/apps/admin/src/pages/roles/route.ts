import { inject } from '@angular/core';
import { Routes } from '@angular/router';
import { Common } from '../../services/common';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./roles'),
    canActivate:[()=>inject(Common).checkPermissionForRouting('role:view')]
  },
  {
    path: 'add',
    loadComponent: () => import('./create/create'),
    canActivate:[()=>inject(Common).checkPermissionForRouting('role:create')]
  },
  {
    path: 'edit/:id',
    loadComponent: () => import('./create/create'),
    canActivate:[()=>inject(Common).checkPermissionForRouting('role:edit')]
  },
  {
    path: 'details/:id',
    loadComponent: () => import('./detail/detail'),
    canActivate:[()=>inject(Common).checkPermissionForRouting('role:view')]
  },
  {
    path: 'permissions/:id',
    loadComponent: () => import('./permissions/permissions'),
    canActivate:[()=>inject(Common).checkPermissionForRouting('role:update_permissions')]
  },
];

export default routes;
