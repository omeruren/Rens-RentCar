import { CommonModule } from '@angular/common';
import { httpResource } from '@angular/common/http';
import {
  ChangeDetectionStrategy,
  Component,
  computed,
  effect,
  inject,
  signal,
  ViewEncapsulation,
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import Blank from 'apps/admin/src/components/blank/blank';
import {
  VehicleModel,
  INITIAL_VEHICLE_MODEL,
} from 'apps/admin/src/models/vehicle.model';
import { Result } from 'apps/admin/src/models/result.model';
import {
  BreadCrumbModel,
  BreadcrumbService,
} from 'apps/admin/src/services/breadcrumb';

@Component({
  imports: [Blank, CommonModule],
  templateUrl: './detail.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class Detail {
  //  <-- Services -->
  readonly #activated = inject(ActivatedRoute);
  readonly #breadcrumb = inject(BreadcrumbService);

  readonly id = signal<string>('');
  readonly pageTitle = computed(
    () => `${this.data().brand} ${this.data().model}`
  );
  constructor() {
    this.#activated.params.subscribe((res) => {
      this.id.set(res['id']);
    });

    effect(() => {
      const breadcrumbList: BreadCrumbModel[] = [
        {
          title: 'Vehicles',
          url: '/vehicles',
          icon: 'bi-car-front',
        },
      ];

      if (this.data()) {
        this.breadcrumbs.set(breadcrumbList);
        this.breadcrumbs.update((prev) => [
          ...prev,
          {
            title: `${this.data().brand} ${this.data().model}`,
            icon: 'bi-zoom-in',
            url: `/vehicles/details/${this.id()}`,
          },
        ]);
        this.#breadcrumb.reset(this.breadcrumbs());
      }
    });
  }

  readonly result = httpResource<Result<VehicleModel>>(
    () => `rent/vehicles/${this.id()}`
  );
  readonly data = computed(
    () => this.result.value()?.data ?? INITIAL_VEHICLE_MODEL
  );

  readonly loading = computed(() => this.result.isLoading());

  readonly breadcrumbs = signal<BreadCrumbModel[]>([]);
}
