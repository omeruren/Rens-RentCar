import {
  ChangeDetectionStrategy,
  Component,
  inject,
  signal,
  ViewEncapsulation,
} from '@angular/core';
import Grid from '../../components/grid/grid';
import { FlexiGridModule } from 'flexi-grid';
import { BreadCrumbModel, BreadcrumbService } from '../../services/breadcrumb';

@Component({
  imports: [Grid, FlexiGridModule],
  templateUrl: './vehicles.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class Vehicles {
  // <-- Services -->
  readonly #breadcrumb = inject(BreadcrumbService);

  constructor() {
    this.#breadcrumb.reset(this.breadcrumbs());
  }

  readonly breadcrumbs = signal<BreadCrumbModel[]>([
    {
      title: 'Vehicles',
      url: '/vehicles',
      icon: 'bi-car-front',
      isActive: true,
    },
  ]);
}
