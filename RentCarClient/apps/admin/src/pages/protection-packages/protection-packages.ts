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
  templateUrl: './protection-packages.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class ProtectionPackages {
  // <-- Services -->
  readonly #breadcrumb = inject(BreadcrumbService);
  constructor() {
    this.#breadcrumb.reset(this.breadcrumbs());
  }

  readonly breadcrumbs = signal<BreadCrumbModel[]>([
    {
      title: 'Protection Packages',
      icon: 'bi-shield',
      url: '/protection-packages',
      isActive: true,
    },
  ]);
}
