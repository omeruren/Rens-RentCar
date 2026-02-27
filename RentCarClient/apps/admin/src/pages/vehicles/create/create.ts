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
import { BranchModel } from 'apps/admin/src/models/branch.model';
import { CategoryModel } from 'apps/admin/src/models/category.model';
import { ODataModel } from 'apps/admin/src/models/odata.model';
import {
  VehicleModel,
  INITIAL_VEHICLE_MODEL,
} from 'apps/admin/src/models/vehicle.model';
import {
  BreadcrumbService,
  BreadCrumbModel,
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
            url: '/vehicles/add',
            isActive: true,
          },
        ]);
        this.#breadcrumb.reset(this.breadcrumbs());
      }
    });

    // Initialize featuresText when data changes
    effect(() => {
      if (this.data().features && this.data().features.length > 0) {
        this.featuresText.set(this.data().features.join(', '));
      }
    });
  }

  readonly result = resource({
    params: () => this.id(),
    loader: async () => {
      var res = await lastValueFrom(
        this.#http.getResource<VehicleModel>(`rent/vehicles/${this.id()}`)
      );
      this.breadcrumbs.update((prev) => [
        ...prev,
        {
          title: `${res.data!.brand} ${res.data!.model}`,
          icon: 'bi-pen',
          url: `/vehicles/edit/${this.id()}`,
          isActive: true,
        },
      ]);
      this.#breadcrumb.reset(this.breadcrumbs());

      return res.data;
    },
  });

  readonly categoryResult = httpResource<ODataModel<CategoryModel>>(
    () => 'rent/odata/categories'
  );
  readonly categories = computed(
    () => this.categoryResult.value()?.value ?? []
  );

  readonly branchResult = httpResource<ODataModel<BranchModel>>(
    () => 'rent/odata/branches'
  );
  readonly branches = computed(() => this.branchResult.value()?.value ?? []);

  readonly loading = linkedSignal(() => this.result.isLoading());

  readonly data = linkedSignal(
    () => this.result.value() ?? { ...INITIAL_VEHICLE_MODEL }
  );

  readonly featuresText = signal<string>('');

  readonly pageTitle = computed(() =>
    this.id() ? 'Edit Vehicle' : 'Add Vehicle'
  );
  readonly pageIcon = computed(() => (this.id() ? ' bi-pen' : ' bi-plus '));
  readonly id = signal<string | undefined>(undefined);

  readonly breadcrumbs = signal<BreadCrumbModel[]>([
    {
      title: 'Vehicles',
      icon: 'bi-car-front',
      url: '/vehicles',
    },
  ]);

  changeStatus(status: boolean) {
    this.data.update((prev) => ({
      ...prev,
      isActive: status,
    }));
  }

  save(form: NgForm) {
    if (!form.valid) return;
    this.loading.set(true);

    // Convert featuresText to features array
    const featuresArray = this.featuresText()
      .split(',')
      .map((f) => f.trim())
      .filter((f) => f.length > 0);

    this.data.update((prev) => ({
      ...prev,
      features: featuresArray,
    }));

    if (!this.id()) {
      this.#http.post<string>(
        'rent/vehicles',
        this.data(),
        (res) => {
          this.loading.set(false);
          this.#toast.showToast('Success', res, 'success');
          this.#router.navigateByUrl('/vehicles');
        },
        () => this.loading.set(false)
      );
    } else {
      this.#http.put<string>(
        'rent/vehicles',
        this.data(),
        (res) => {
          this.loading.set(false);
          this.#toast.showToast('Success', res, 'info');
          this.#router.navigateByUrl('/vehicles');
        },
        () => this.loading.set(false)
      );
    }
  }
}
