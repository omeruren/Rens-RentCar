import { inject } from '@angular/core';
import { Routes } from '@angular/router';
import { Common } from '../../services/common';

const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./vehicles'),
    canActivate: [
      () => inject(Common).checkPermissionForRouting('vehicle:view'),
    ],
  },
  {
    path: 'add',
    loadComponent: () => import('./create/create'),
    canActivate: [
      () => inject(Common).checkPermissionForRouting('vehicle:create'),
    ],
  },
  {
    path: 'edit/:id',
    loadComponent: () => import('./create/create'),
    canActivate: [
      () => inject(Common).checkPermissionForRouting('vehicle:edit'),
    ],
  },
  {
    path: 'details/:id',
    loadComponent: () => import('./detail/detail'),
    canActivate: [
      () => inject(Common).checkPermissionForRouting('vehicle:view'),
    ],
  },
];
export default routes;
