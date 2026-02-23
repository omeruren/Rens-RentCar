import { httpResource } from '@angular/common/http';
import {
  ChangeDetectionStrategy,
  Component,
  computed,
  effect,
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
  BranchModel,
  INITIAL_BRANCH_MODEL,
} from 'apps/admin/src/models/branch.model';
import {
  BreadCrumbModel,
  BreadcrumbService,
} from 'apps/admin/src/services/breadcrumb';
import { HttpService } from 'apps/admin/src/services/http';
import { FlexiSelectModule } from 'flexi-select';
import { FlexiToastService } from 'flexi-toast';
import { FormValidateDirective } from 'form-validate-angular';
import { NgxMaskDirective } from 'ngx-mask';
import { lastValueFrom } from 'rxjs';

@Component({
  imports: [
    Blank,
    FormsModule,
    FormValidateDirective,
    NgxMaskDirective,
    FlexiSelectModule,
  ],
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
            url: '/branches/add',
            isActive: true,
          },
        ]);
        this.#breadcrumb.reset(this.breadcrumbs());
      }
    });
    effect(() => {
      if (this.data().city) this.loadDistricts();
    });
  }

  readonly cityDistrictResult = httpResource<any[]>(() => '/il-ilce.json');
  readonly result = resource({
    params: () => this.id(),
    loader: async () => {
      var res = await lastValueFrom(
        this.#http.getResource<BranchModel>(`rent/branches/${this.id()}`)
      );
      this.breadcrumbs.update((prev) => [
        ...prev,
        {
          title: res.data!.name,
          icon: 'bi-pen',
          url: `/branches/edit/${this.id()}`,
          isActive: true,
        },
      ]);
      this.#breadcrumb.reset(this.breadcrumbs());

      return res.data;
    },
  });

  readonly loading = linkedSignal(() => this.result.isLoading());
  readonly cityLoading = computed(() => this.cityDistrictResult.isLoading());

  readonly data = linkedSignal(
    () => this.result.value() ?? { ...INITIAL_BRANCH_MODEL }
  );
  readonly cities = computed(() => this.cityDistrictResult.value() ?? []);

  readonly pageTitle = computed(() =>
    this.id() ? 'Edit Branch' : 'Add Branch'
  );
  readonly pageIcon = computed(() => (this.id() ? ' bi-pen' : ' bi-plus '));
  readonly id = signal<string | undefined>(undefined);
  readonly districts = signal<any[]>([]);

  readonly breadcrumbs = signal<BreadCrumbModel[]>([
    {
      title: 'Branches',
      icon: 'bi-buildings',
      url: '/branches',
    },
  ]);

  save(form: NgForm) {
    if (!form.valid) return;
    this.loading.set(true);

    if (!this.id()) {
      this.#http.post<string>(
        'rent/branches',
        this.data(),
        (res) => {
          this.loading.set(false);
          this.#toast.showToast('Success', res, 'success');
          this.#router.navigateByUrl('/branches');
        },
        () => this.loading.set(false)
      );
    } else {
      this.#http.put<string>(
        'rent/branches',
        this.data(),
        (res) => {
          this.loading.set(false);
          this.#toast.showToast('Success', res, 'info');
          this.#router.navigateByUrl('/branches');
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

  loadDistricts() {
    const city = this.cities().find((i) => i.il_adi === this.data().city);
    this.districts.set(city.ilceler);
  }
}
