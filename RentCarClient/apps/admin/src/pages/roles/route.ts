import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./roles'),
  },
  {
    path: 'add',
    loadComponent: () => import('./create/create'),
  },
  {
    path: 'edit/:id',
    loadComponent: () => import('./create/create'),
  },
  {
    path: 'details/:id',
    loadComponent: () => import('./detail/detail'),
  },
  {
    path: 'permissions/:id',
    loadComponent: () => import('./permissions/permissions'),
  },
];

export default routes;
