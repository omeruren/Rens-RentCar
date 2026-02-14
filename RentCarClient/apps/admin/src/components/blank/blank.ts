import { DatePipe, Location, NgClass } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  inject,
  input,
  ViewEncapsulation,
} from '@angular/core';
import { RouterLink } from '@angular/router';
import { BaseEntityModel } from '../../models/base.entity';

@Component({
  selector:'app-blank',
  imports: [NgClass, RouterLink, DatePipe],
  templateUrl: './blank.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class Blank {
  //  <-- Services -->
  readonly #location = inject(Location);
  readonly pageTitle = input.required<string>();
  readonly pageIcon = input.required<string>();
  readonly pageDescription = input<string>('');
  readonly showStatus = input<boolean>(false);
  readonly status = input<boolean>(true);
  readonly showBackButton = input<boolean>(true);
  readonly showEditButton = input<boolean>(false);
  readonly editButtonUrl = input<string>('');
  readonly showAudit = input<boolean>(false);
  readonly audit = input<BaseEntityModel>();
  return() {
    this.#location.back();
  }
}
