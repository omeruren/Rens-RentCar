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
];
export default rotes;
