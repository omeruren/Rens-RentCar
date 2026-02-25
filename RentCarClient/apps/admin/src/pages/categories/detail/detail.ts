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
  CategoryModel,
  INITIAL_CATEGORY_MODEL,
} from 'apps/admin/src/models/category.model';
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
  readonly pageTitle = computed(() => this.data().name);
  constructor() {
    this.#activated.params.subscribe((res) => {
      this.id.set(res['id']);
    });

    effect(() => {
      const breadcrumbList: BreadCrumbModel[] = [
        {
          title: 'Categories',
          icon: 'bi-tag',
          url: '/categories',
        },
      ];

      if (this.data()) {
        this.breadcrumbs.set(breadcrumbList);
        this.breadcrumbs.update((prev) => [
          ...prev,
          {
            title: this.data().name,
            icon: 'bi-zoom-in',
            url: `/categories/details/${this.id()}`,
          },
        ]);
        this.#breadcrumb.reset(this.breadcrumbs());
      }
    });
  }

  readonly result = httpResource<Result<CategoryModel>>(
    () => `rent/categories/${this.id()}`
  );
  readonly data = computed(
    () => this.result.value()?.data ?? INITIAL_CATEGORY_MODEL
  );

  readonly loading = computed(() => this.result.isLoading());

  readonly breadcrumbs = signal<BreadCrumbModel[]>([]);
}
