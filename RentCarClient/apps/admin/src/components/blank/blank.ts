import { DatePipe, Location, NgClass } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  inject,
  input,
  output,
  ViewEncapsulation,
} from '@angular/core';
import { RouterLink } from '@angular/router';
import { BaseEntityModel } from '../../models/base.entity';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-blank',
  imports: [NgClass, RouterLink, DatePipe, FormsModule],
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
  readonly showStatusCheckbox = input<boolean>(false);
  readonly changeStatusEvent = output<boolean>();
  return() {
    this.#location.back();
  }

  changeStatus(event: any) {
    const checked = event.target.checked;
    this.changeStatusEvent.emit(checked);
  }
}
