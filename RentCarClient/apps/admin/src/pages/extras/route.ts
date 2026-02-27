import { inject } from '@angular/core';
import { Routes } from '@angular/router';
import { Common } from '../../services/common';

const rotes: Routes = [
  {
    path: '',
    loadComponent: () => import('./extras'),
    canActivate: [() => inject(Common).checkPermissionForRouting('extra:view')],
  },
  {
    path: 'add',
    loadComponent: () => import('./create/create'),
    canActivate: [
      () => inject(Common).checkPermissionForRouting('extra:create'),
    ],
  },
  {
    path: 'edit/:id',
    loadComponent: () => import('./create/create'),
    canActivate: [
      () => inject(Common).checkPermissionForRouting('extra:edit'),
    ],
  },
  {
    path: 'details/:id',
    loadComponent: () => import('./detail/detail'),
    canActivate: [
      () => inject(Common).checkPermissionForRouting('extra:view'),
    ],
  },
];
export default rotes;
