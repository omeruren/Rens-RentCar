import { inject } from '@angular/core';
import { Routes } from '@angular/router';
import { Common } from '../../services/common';

const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./reservations'),
    canActivate: [
      () => inject(Common).checkPermissionForRouting('reservation:view'),
    ],
  },
  {
    path:'add',
    loadComponent:()=>import('./create/create'),
    canActivate:[()=>inject(Common).checkPermissionForRouting('reservation:create')]
  }
];
export default routes;
