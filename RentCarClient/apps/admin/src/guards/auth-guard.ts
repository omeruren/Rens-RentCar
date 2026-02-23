import { inject } from '@angular/core';
import { CanActivateChildFn, Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { Common } from '../services/common';

export const authGuard: CanActivateChildFn = (childRoute, state) => {
  // <-- Services -->
  const router = inject(Router);
  const common = inject(Common);

  const token = localStorage.getItem('response');
  if (!token) {
    router.navigateByUrl('/login');
    return false;
  }

  try {
    const decodedToken: any = jwtDecode(token); // decode token

    common.token().id =
      decodedToken[
        'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'
      ];
    common.token().fullName = decodedToken['fullName'];
    common.token().fullNameWithEmail = decodedToken['fullnameWithEmail'];
    common.token().email = decodedToken['email'];
    common.token().role = decodedToken['role'];
    common.token().permissions = JSON.parse(decodedToken['permissions']);
    common.token().branch =decodedToken['branch'];

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
