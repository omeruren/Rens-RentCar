import { Injectable, signal } from '@angular/core';

export interface BreadCrumbModel {
  title: string;
  url: string;
  icon: string;
  isActive?: boolean;
}
@Injectable({
  providedIn: 'root',
})
export class BreadcrumbService {
  data = signal<BreadCrumbModel[]>([]);

  reset() {
    const dashboard: BreadCrumbModel = {
      title: 'Dashboard',
      url: '/',
      icon: 'bi-speedometer2',
    };
    this.data.set([{ ...dashboard }]);
  }

  setDashboard() {
    const dashboard: BreadCrumbModel = {
      title: 'Dashboard',
      url: '/',
      icon: 'bi-speedometer2',
      isActive: true,
    };
    this.data.set([{ ...dashboard }]);
  }

  set(breadcrumbs: BreadCrumbModel[]) {
    this.data.update((prev) => [...prev, ...breadcrumbs]);
  }
}
