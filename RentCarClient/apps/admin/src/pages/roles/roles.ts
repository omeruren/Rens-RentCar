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

@Component({
  imports: [Grid,FlexiGridModule],
  templateUrl: './roles.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class Roles {
  // <-- Services -->
  readonly #breadcrumb = inject(BreadcrumbService);

  readonly breadcrumbs = signal<BreadCrumbModel[]>([
    {
      title: 'Roles',
      icon: 'bi-person-lock',
      url: '/roles',
      isActive: true,
    },
  ]);


}
