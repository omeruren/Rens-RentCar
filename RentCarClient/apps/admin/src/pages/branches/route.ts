import { Routes } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./branches'),
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
];

export default routes;
