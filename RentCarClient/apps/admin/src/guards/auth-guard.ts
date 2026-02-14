import { inject } from '@angular/core';
import { CanActivateChildFn, Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

export const authGuard: CanActivateChildFn = (childRoute, state) => {
  // <-- Services -->
  const router = inject(Router);
  const token = localStorage.getItem('response');
  if (!token) {
    router.navigateByUrl('/login');
    return false;
  }

  try {
    const decodedToken = jwtDecode(token); // decode token

    const now = new Date().getTime() / 1000;
    const exp = decodedToken.exp ?? 0;
    if (exp! <= now) {
      router.navigateByUrl('/login');
      return false;
    }
    return true;
  } catch (error) {
    router.navigateByUrl('/login');
  }
  return true;
};
