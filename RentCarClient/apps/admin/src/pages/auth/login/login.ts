import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  inject,
  signal,
  viewChild,
  ViewEncapsulation,
} from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { FormValidateDirective } from 'form-validate-angular';
import { HttpService } from '../../../services/http';
import { FlexiToastService } from 'flexi-toast';

@Component({
  imports: [FormsModule, FormValidateDirective],
  templateUrl: './login.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class Login {
  //  <-- Services -->
  readonly #http = inject(HttpService);
  readonly #router = inject(Router);
  readonly #toast = inject(FlexiToastService);

  readonly loading = signal<boolean>(false);
  readonly email = signal<string>('');
  readonly closeModalBtn =
    viewChild<ElementRef<HTMLButtonElement>>('modalCloseButton');
  readonly passwordEl = viewChild<ElementRef<HTMLInputElement>>('passwordEl');

  togglePasswordVisibility() {
    this.passwordEl()?.nativeElement.type === 'password'
      ? this.passwordEl()?.nativeElement.setAttribute('type', 'text')
      : this.passwordEl()?.nativeElement.setAttribute('type', 'password');
  }

  //  SIGNIN

  signIn(form: NgForm) {
    if (!form.valid) return;
    this.loading.set(true);
    this.#http.post<string>(
      'rent/auth/login',
      form.value,
      (res) => {
        localStorage.setItem('response', res);
        this.#router.navigateByUrl('/');
        this.loading.set(false);
      },
      () => this.loading.set(false)
    );
  }

  //FORGOT PASSWORD

  forgotPassword() {
    this.#http.post<string>(
      `rent/auth/forgot-password/${this.email()}`,
      {},
      (res) => {
        this.#toast.showToast('Success', res, 'info');
        this.closeModalBtn()!.nativeElement.click();
      }
    );
  }
}
