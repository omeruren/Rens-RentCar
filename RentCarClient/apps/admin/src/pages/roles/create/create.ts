import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  linkedSignal,
  resource,
  signal,
  ViewEncapsulation,
} from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import Blank from 'apps/admin/src/components/blank/blank';
import {
  RoleModel,
  INITIAL_ROLE_MODEL,
} from 'apps/admin/src/models/role.model';
import {
  BreadCrumbModel,
  BreadcrumbService,
} from 'apps/admin/src/services/breadcrumb';
import { HttpService } from 'apps/admin/src/services/http';
import { FlexiToastService } from 'flexi-toast';
import { FormValidateDirective } from 'form-validate-angular';
import { NgxMaskDirective } from 'ngx-mask';
import { lastValueFrom } from 'rxjs';

@Component({
  imports: [Blank, FormsModule, FormValidateDirective],
  templateUrl: './create.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class Create {
  //  <-- Services -->
  readonly #breadcrumb = inject(BreadcrumbService);
  readonly #activated = inject(ActivatedRoute);
  readonly #router = inject(Router);
  readonly #toast = inject(FlexiToastService);
  readonly #http = inject(HttpService);

  constructor() {
    this.#activated.params.subscribe((res) => {
      if (res['id']) this.id.set(res['id']);
      else {
        this.breadcrumbs.update((prev) => [
          ...prev,
          {
            title: 'Add',
            icon: 'bi-plus',
            url: '/roles/add',
            isActive: true,
          },
        ]);
        this.#breadcrumb.reset(this.breadcrumbs());
      }
    });
  }

  readonly result = resource({
    params: () => this.id(),
    loader: async () => {
      var res = await lastValueFrom(
        this.#http.getResource<RoleModel>(`rent/roles/${this.id()}`)
      );
      this.breadcrumbs.update((prev) => [
        ...prev,
        {
          title: res.data!.name,
          icon: 'bi-pen',
          url: `/roles/edit/${this.id()}`,
          isActive: true,
        },
      ]);
      this.#breadcrumb.reset(this.breadcrumbs());

      return res.data;
    },
  });
  readonly loading = linkedSignal(() => this.result.isLoading());
  readonly data = linkedSignal(() => this.result.value() ?? {...INITIAL_ROLE_MODEL});

  readonly pageTitle = computed(() =>
    this.id() ? 'Edit Role' : 'Add Role'
  );
  readonly pageIcon = computed(() => (this.id() ? ' bi-pen' : ' bi-plus '));
  readonly id = signal<string | undefined>(undefined);

  readonly breadcrumbs = signal<BreadCrumbModel[]>([
    {
      title: 'Roles',
      icon: 'bi-person-lock',
      url: '/roles',
    },
  ]);

  save(form: NgForm) {
    if (!form.valid) return;
    this.loading.set(true);

    if (!this.id()) {
      this.#http.post<string>(
        'rent/roles',
        this.data(),
        (res) => {
          this.loading.set(false);
          this.#toast.showToast('Success', res, 'success');
          this.#router.navigateByUrl('/roles');
        },
        () => this.loading.set(false)
      );
    } else {
      this.#http.put<string>(
        'rent/roles',
        this.data(),
        (res) => {
          this.loading.set(false);
          this.#toast.showToast('Success', res, 'info');
          this.#router.navigateByUrl('/roles');
        },
        () => this.loading.set(false)
      );
    }
  }
  changeStatus(status: boolean) {
    this.data.update((prev) => ({
      ...prev,
      isActive: status,
    }));
  }
}
