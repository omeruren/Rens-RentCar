import { HttpErrorResponse } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { FlexiToastService } from 'flexi-toast';
import { of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ErrorService {
  //  <-- Services -->
  readonly #toast = inject(FlexiToastService);
  readonly #router = inject(Router);
  handle(err: HttpErrorResponse) {
    const status = err.status;

    if (status === 403 || status === 422 || status === 500) {
      const messages = err.error.errorMessages;

      messages.forEach((val: string) => {
        this.#toast.showToast('Error!', val, 'error');
      });
    } else if (status === 401) {
      this.#toast.showToast(
        'Error!',
        'Something went wrong. Please login again.',
        'error'
      );
      localStorage.removeItem('response');
      this.#router.navigateByUrl('/login');
    } else if (status !== 200) {
      this.#toast.showToast(
        'Error!',
        'Something went wrong. Please contact to Administrators',
        'error'
      );
    }
  }
}
