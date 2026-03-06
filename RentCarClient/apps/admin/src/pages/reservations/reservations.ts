import { BreadCrumbModel } from './../../services/breadcrumb';
import {
  ChangeDetectionStrategy,
  Component,
  inject,
  signal,
  ViewEncapsulation,
} from '@angular/core';
import Grid from '../../components/grid/grid';
import { FlexiGridModule } from 'flexi-grid';
import { Common } from '../../services/common';

@Component({
  imports: [Grid, FlexiGridModule],
  templateUrl: './reservations.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class Reservations {
  readonly breadcrumbs = signal<BreadCrumbModel[]>([
    {
      title: 'Reservations',
      url: '/reservations',
      icon: 'bi-calendar-check',
    },
  ]);
}
