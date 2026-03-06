import { BreadCrumbModel } from './../../services/breadcrumb';
import {
  ChangeDetectionStrategy,
  Component,
  signal,
  ViewEncapsulation,
} from '@angular/core';
import Grid from '../../components/grid/grid';
import { FlexiGridFilterDataModel, FlexiGridModule } from 'flexi-grid';
import { NgClass } from '@angular/common';
import { NgxMaskPipe } from 'ngx-mask';

@Component({
  imports: [Grid, FlexiGridModule, NgClass, NgxMaskPipe],
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
      isActive: true,
    },
  ]);

  readonly statusFilterData = signal<FlexiGridFilterDataModel[]>([
    {
      name: 'Pending',
      value: 'Pending',
    },
    {
      name: 'Delivered',
      value: 'Delivered',
    },
    {
      name: 'Completed',
      value: 'Completed',
    },
    {
      name: 'Cancelled',
      value: 'Cancelled',
    },
  ]);

  getStatusClass(status: string) {
    switch (status) {
      case 'Pending':
        return 'flexi-grid-card-warning';
      case 'Delivered':
        return 'flexi-grid-card-info';
      case 'Completed':
        return 'flexi-grid-card-success';
      case 'Cancelled':
        return 'flexi-grid-card-danger';
      default:
        return '';
    }
  }
}
