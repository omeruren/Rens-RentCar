import { inject } from '@angular/core';
import { Routes } from '@angular/router';
import { Common } from '../../services/common';

const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./reservations'),
    canActivate: [
      () => inject(Common).checkPermissionForRouting('vehicle:view'),
    ],
  },
];
export default routes;
