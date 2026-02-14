import { HttpInterceptorFn } from '@angular/common/http';

export const httpInterceptor: HttpInterceptorFn = (req, next) => {
  const url = req.url;
  const serverEndpoint = `https://localhost:7203`;

  let clone = req.clone({
    url: url.replace('rent', serverEndpoint),
  });

  return next(clone);
};
