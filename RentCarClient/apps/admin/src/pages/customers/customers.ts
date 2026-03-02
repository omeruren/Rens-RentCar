import { BreadCrumbModel, BreadcrumbService } from '../../services/breadcrumb';
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
  templateUrl: './customers.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class Customers {
  // <-- Services -->
  readonly #breadcrumb = inject(BreadcrumbService);
  constructor() {
    this.#breadcrumb.reset(this.breadcrumbs());
  }

  readonly breadcrumbs = signal<BreadCrumbModel[]>([
    {
      title: 'Customers',
      icon: 'bi-people',
      url: '/customers',
      isActive: true,
    },
  ]);
}
