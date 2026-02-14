import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, of } from 'rxjs';
import { Error as ErrorService } from '../services/error';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  // <-- Services -->
  const error = inject(ErrorService);

  return next(req).pipe(
    catchError((err: HttpErrorResponse) => {
      error.handle(err);

      return of();
    })
  );
};
