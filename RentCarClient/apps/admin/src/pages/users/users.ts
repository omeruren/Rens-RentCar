import {
  ChangeDetectionStrategy,
  Component,
  inject,
  signal,
  ViewEncapsulation,
} from '@angular/core';
import Grid from '../../components/grid/grid';
import { BreadCrumbModel, BreadcrumbService } from '../../services/breadcrumb';
import { FlexiGridModule } from "flexi-grid";

@Component({
  imports: [Grid, FlexiGridModule],
  templateUrl: './users.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class Users {
  //  <-- Services -->
  readonly #breadcrumb = inject(BreadcrumbService);

  readonly breadcrumbs = signal<BreadCrumbModel[]>([
    {
      title: 'Users',
      icon: 'bi-people',
      url: '/users',
      isActive: true,
    },
  ]);
}
