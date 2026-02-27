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
  ExtraModel,
  INITIAL_EXTRA_MODEL,
} from 'apps/admin/src/models/extra.model';
import { Result } from 'apps/admin/src/models/result.model';
import {
  BreadcrumbService,
  BreadCrumbModel,
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
          title: 'Extras',
          url: '/extras',
          icon: 'bi-plus-square',
        },
      ];

      if (this.data()) {
        this.breadcrumbs.set(breadcrumbList);
        this.breadcrumbs.update((prev) => [
          ...prev,
          {
            title: this.data().name,
            icon: 'bi-zoom-in',
            url: `/extras/details/${this.id()}`,
          },
        ]);
        this.#breadcrumb.reset(this.breadcrumbs());
      }
    });
  }

  readonly result = httpResource<Result<ExtraModel>>(
    () => `rent/extras/${this.id()}`
  );
  readonly data = computed(
    () => this.result.value()?.data ?? INITIAL_EXTRA_MODEL
  );

  readonly loading = computed(() => this.result.isLoading());

  readonly breadcrumbs = signal<BreadCrumbModel[]>([]);
}
