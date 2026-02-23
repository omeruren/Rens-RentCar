import {
  ChangeDetectionStrategy,
  Component,
  inject,
  signal,
  ViewEncapsulation,
} from '@angular/core';
import { BreadCrumbModel, BreadcrumbService } from '../../services/breadcrumb';
import Grid from '../../components/grid/grid';
import { FlexiGridModule } from 'flexi-grid';
import { RouterLink } from '@angular/router';
import { Common } from '../../services/common';

@Component({
  imports: [Grid, FlexiGridModule, RouterLink],
  templateUrl: './roles.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class Roles {
  // <-- Services -->
  readonly #common = inject(Common);
  readonly breadcrumbs = signal<BreadCrumbModel[]>([
    {
      title: 'Roles',
      icon: 'bi-person-lock',
      url: '/roles',
      isActive: true,
    },
  ]);

  checkPermission(permission: string) {
    return this.#common.checkPermission(permission);
  }
}
