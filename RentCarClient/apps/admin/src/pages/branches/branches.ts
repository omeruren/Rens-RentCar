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
import Blank from '../../components/blank/blank';
import { NgxMaskPipe } from 'ngx-mask';
import { RouterLink } from '@angular/router';

import Grid from '../../components/grid/grid';
import { FlexiGridModule } from 'flexi-grid';
@Component({
  imports: [Blank, Grid,FlexiGridModule, NgxMaskPipe],
  templateUrl: './branches.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class Branches {
  // <-- Services -->
  readonly #breadcrumb = inject(BreadcrumbService);
  constructor() {
    this.#breadcrumb.reset(this.breadcrumbs());
  }

  readonly breadcrumbs = signal<BreadCrumbModel[]>([
    {
      title: 'Branches',
      icon: 'bi-buildings',
      url: '/branches',
      isActive: true,
    },
  ]);
}
