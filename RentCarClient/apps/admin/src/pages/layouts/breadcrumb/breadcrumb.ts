import {
  ChangeDetectionStrategy,
  Component,
  inject,
  ViewEncapsulation,
} from '@angular/core';
import { BreadcrumbService } from 'apps/admin/src/services/breadcrumb';
import { RouterLink } from '@angular/router';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-breadcrumb',
  imports: [NgClass, RouterLink],
  templateUrl: './breadcrumb.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class Breadcrumb {
  //  <-- Services -->
  readonly breadcrumb = inject(BreadcrumbService);
}
