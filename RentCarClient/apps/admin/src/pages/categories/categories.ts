import {
  BreadCrumbModel,
  BreadcrumbService,
} from './../../services/breadcrumb';
import {
  ChangeDetectionStrategy,
  Component,
  inject,
  signal,
  ViewEncapsulation,
} from '@angular/core';

import Grid from '../../components/grid/grid';
import { FlexiGridModule } from 'flexi-grid';

@Component({
  imports: [Grid, FlexiGridModule],
  templateUrl: './categories.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class Categories {
  // <-- Services -->
  readonly #breadcrumb = inject(BreadcrumbService);
  constructor() {
    this.#breadcrumb.reset(this.breadcrumbs());
  }

  readonly breadcrumbs = signal<BreadCrumbModel[]>([
    {
      title: 'Categories',
      icon: 'bi-tag',
      url: '/categories',
      isActive: true,
    },
  ]);
}
