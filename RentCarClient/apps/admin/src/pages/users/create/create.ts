import { NgClass, registerLocaleData } from '@angular/common';
import { httpResource } from '@angular/common/http';
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
import { BranchModel } from 'apps/admin/src/models/branch.model';
import { ODataModel } from 'apps/admin/src/models/odata.model';
import { RoleModel } from 'apps/admin/src/models/role.model';
import {
  INITIAL_USER_MODEL,
  UserModel,
} from 'apps/admin/src/models/user.model';
import {
  BreadCrumbModel,
  BreadcrumbService,
} from 'apps/admin/src/services/breadcrumb';
import { Common } from 'apps/admin/src/services/common';
import { HttpService } from 'apps/admin/src/services/http';
import { FlexiSelectModule } from 'flexi-select';
import { FlexiToastService } from 'flexi-toast';
import { FormValidateDirective } from 'form-validate-angular';
import { lastValueFrom } from 'rxjs';

@Component({
  imports: [
    Blank,
    FormsModule,
    FormValidateDirective,
    NgClass,
    FlexiSelectModule,
  ],
  templateUrl: './create.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class Create {
  // <-- Services -->
  readonly #breadcrumb = inject(BreadcrumbService);
  readonly #activated = inject(ActivatedRoute);
  readonly #router = inject(Router);
  readonly #toast = inject(FlexiToastService);
  readonly #http = inject(HttpService);
  readonly #common = inject(Common);
  constructor() {
    this.#activated.params.subscribe((res) => {
      if (res['id']) this.id.set(res['id']);
      else {
        this.breadcrumbs.update((prev) => [
          ...prev,
          {
            title: 'Add',
            url: '/users/add',
            icon: 'bi-plus',
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
        this.#http.getResource<UserModel>(`rent/users/${this.id()}`)
      );
      this.breadcrumbs.update((prev) => [
        ...prev,
        {
          title: `${res.data!.firstName} ${res.data!.lastName}`,
          icon: 'bi-pen',
          url: `/users/edit/${this.id()}`,
          isActive: true,
        },
      ]);
      this.#breadcrumb.reset(this.breadcrumbs());

      return res.data;
    },
  });

  readonly branchResult = httpResource<ODataModel<BranchModel>>(
    () => 'rent/odata/branches'
  );
  readonly branches = computed(() => this.branchResult.value()?.value ?? []);
  readonly branchLoading = computed(() => this.branchResult.isLoading());

  readonly roleResult = httpResource<ODataModel<RoleModel>>(
    () => 'rent/odata/roles'
  );
  readonly roles = computed(() => this.roleResult.value()?.value ?? []);
  readonly roleLoading = computed(() => this.roleResult.isLoading());

  readonly loading = linkedSignal(() => this.result.isLoading());
  readonly data = linkedSignal(
    () => this.result.value() ?? { ...INITIAL_USER_MODEL }
  );

  readonly pageTitle = computed(() => (this.id() ? 'Edit User' : 'Add User'));
  readonly pageIcon = computed(() => (this.id() ? 'bi-pen' : 'bi-plus'));

  readonly id = signal<string | undefined>(undefined);

  readonly breadcrumbs = signal<BreadCrumbModel[]>([
    {
      title: 'Users',
      url: '/users',
      icon: 'bi-people',
    },
  ]);

  save(form: NgForm) {
    if (!form.valid) return;
    this.loading.set(true);

    if (!this.id()) {
      this.#http.post<string>(
        'rent/users',
        this.data(),
        (res) => {
          this.loading.set(false);
          this.#toast.showToast('Success', res, 'success');
          this.#router.navigateByUrl('/users');
        },
        () => this.loading.set(false)
      );
    } else {
      this.#http.put<string>(
        'rent/users',
        this.data(),
        (res) => {
          this.loading.set(false);
          this.#toast.showToast('Success', res, 'info');
          this.#router.navigateByUrl('/users');
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

  isAdmin() {
    return this.#common.token().role === 'sys_admin';
  }
}
