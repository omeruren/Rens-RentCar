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
  CustomerModel,
  INITIAL_CUSTOMER_MODEL,
} from 'apps/admin/src/models/customer.model';
import { Result } from 'apps/admin/src/models/result.model';
import {
  BreadCrumbModel,
  BreadcrumbService,
} from 'apps/admin/src/services/breadcrumb';

@Component({
  imports: [Blank],
  templateUrl: './detail.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class Detail {
  //  <-- Services -->
  readonly #activated = inject(ActivatedRoute);
  readonly #breadcrumb = inject(BreadcrumbService);

  readonly id = signal<string>('');
  readonly pageTitle = computed(() => this.data().fullName);
  constructor() {
    this.#activated.params.subscribe((res) => {
      this.id.set(res['id']);
    });

    effect(() => {
      const breadcrumbList: BreadCrumbModel[] = [
        {
          title: 'Customers',
          icon: 'bi-people',
          url: '/customers',
        },
      ];

      if (this.data()) {
        this.breadcrumbs.set(breadcrumbList);
        this.breadcrumbs.update((prev) => [
          ...prev,
          {
            title: this.data().fullName,
            icon: 'bi-zoom-in',
            url: `/customers/details/${this.id()}`,
          },
        ]);
        this.#breadcrumb.reset(this.breadcrumbs());
      }
    });
  }

  readonly result = httpResource<Result<CustomerModel>>(
    () => `rent/customers/${this.id()}`
  );
  readonly data = computed(
    () => this.result.value()?.data ?? INITIAL_CUSTOMER_MODEL
  );

  readonly loading = computed(() => this.result.isLoading());

  readonly breadcrumbs = signal<BreadCrumbModel[]>([]);
}
