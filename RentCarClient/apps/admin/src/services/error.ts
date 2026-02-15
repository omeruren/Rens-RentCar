import { HttpErrorResponse } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { FlexiToastService } from 'flexi-toast';
import { of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ErrorService {
  //  <-- Services -->
  readonly #toast = inject(FlexiToastService);

  handle(err: HttpErrorResponse) {
    const status = err.status;

    if (status === 403 || status === 422 || status === 500) {
      const messages = err.error.errorMessages;

      messages.forEach((val: string) => {
        this.#toast.showToast('Error!', val, 'error');
      });
    }

    return of();
  }
}
