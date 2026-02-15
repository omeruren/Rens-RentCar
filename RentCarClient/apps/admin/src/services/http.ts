import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { ErrorService } from './error';
import { Result } from '../models/result.model';

@Injectable({
  providedIn: 'root',
})
export class HttpService {
  //  <-- Services -->
  readonly #http = inject(HttpClient);
  readonly #errorService = inject(ErrorService);

  // GET

  get<T>(
    endpoint: string,
    callback: (res: T) => void,
    errorCallback?: (err: HttpErrorResponse) => void
  ) {
    this.#http.get<Result<T>>(endpoint).subscribe({
      next: (res) => {
        callback(res.data!);
      },
      error: (err: HttpErrorResponse) => {
        this.#errorService.handle(err);
        if (errorCallback) errorCallback(err);
      },
    });
  }

  // POST

  post<T>(
    endpoint: string,
    body: any,
    callback: (res: T) => void,
    errorCallback?: (err: HttpErrorResponse) => void
  ) {
    this.#http.post<Result<T>>(endpoint, body).subscribe({
      next: (res) => {
        callback(res.data!);
      },
      error: (err: HttpErrorResponse) => {
        this.#errorService.handle(err);
        if (errorCallback) errorCallback(err);
      },
    });
  }

  // PUT

  put<T>(
    endpoint: string,
    body: any,
    callback: (res: T) => void,
    errorCallback?: (err: HttpErrorResponse) => void
  ) {
    this.#http.put<Result<T>>(endpoint, body).subscribe({
      next: (res) => {
        callback(res.data!);
      },
      error: (err: HttpErrorResponse) => {
        this.#errorService.handle(err);
        if (errorCallback) errorCallback(err);
      },
    });
  }

  // DELETE

  delete<T>(
    endpoint: string,

    callback: (res: T) => void,
    errorCallback?: (err: HttpErrorResponse) => void
  ) {
    this.#http.delete<Result<T>>(endpoint).subscribe({
      next: (res) => {
        callback(res.data!);
      },
      error: (err: HttpErrorResponse) => {
        this.#errorService.handle(err);
        if (errorCallback) errorCallback(err);
      },
    });
  }
}
