import {
  ApplicationConfig,
  provideZonelessChangeDetection,
} from '@angular/core';
import { provideRouter } from '@angular/router';
import { appRoutes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { httpInterceptor } from './interceptors/http-interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZonelessChangeDetection(),
    provideRouter(appRoutes),
    provideHttpClient(withInterceptors([httpInterceptor])),
  ],
};
