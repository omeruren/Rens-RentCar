import { inject } from '@angular/core';
import { Routes } from '@angular/router';
import { Common } from '../../services/common';

const router: Routes = [
  {
    path: '',
    loadComponent: () => import('./users'),
    canActivate: [() => inject(Common).checkPermissionForRouting('user:view')],
  },
];
export default router;
