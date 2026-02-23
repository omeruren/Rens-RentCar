import { inject, Injectable, signal } from '@angular/core';
import { INITIAL_TOKEN_MODEL, TokenModel } from '../models/token.model';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class Common {
  //  <-- Services -->
  readonly #router = inject(Router);
  readonly token = signal<TokenModel>(INITIAL_TOKEN_MODEL);

  checkPermission(permission: string) {
    // if (this.token().role === 'sys_admin') return true;

    if (this.token().permissions.some((i) => i === permission)) return true;

    return false;
  }

  checkPermissionForRouting(permission: string) {
    const res = this.checkPermission(permission);

    if (!res) {
      this.#router.navigateByUrl('/unauthorize');
    }

    return res;
  }
}
