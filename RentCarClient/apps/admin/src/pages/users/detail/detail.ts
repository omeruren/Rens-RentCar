import { httpResource } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, computed, effect, inject, signal, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import Blank from 'apps/admin/src/components/blank/blank';
import { Result } from 'apps/admin/src/models/result.model';
import { INITIAL_USER_MODEL, UserModel } from 'apps/admin/src/models/user.model';
import { BreadcrumbService, BreadCrumbModel } from 'apps/admin/src/services/breadcrumb';

@Component({
  imports: [Blank],
  templateUrl: './detail.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Detail {
 //  <-- Services -->
  readonly #activated = inject(ActivatedRoute);
  readonly #breadcrumb = inject(BreadcrumbService);

  readonly id = signal<string>('');
  readonly pageTitle = computed(() =>`${ this.data().firstName} ${this.data().lastName}`);
  constructor() {
    this.#activated.params.subscribe((res) => {
      this.id.set(res['id']);
    });

    effect(() => {
      const breadcrumbList: BreadCrumbModel[] = [
        {
          title: 'users',
          icon: 'bi-people',
          url: '/users',
        },
      ];

      if (this.data()) {
        this.breadcrumbs.set(breadcrumbList);
        this.breadcrumbs.update((prev) => [
          ...prev,
          {
            title: `${ this.data().firstName} ${this.data().lastName}`,
            icon: 'bi-zoom-in',
            url: `/users/details/${this.id()}`,
          },
        ]);
        this.#breadcrumb.reset(this.breadcrumbs());
      }
    });
  }

  readonly result = httpResource<Result<UserModel>>(
    () => `rent/users/${this.id()}`
  );
  readonly data = computed(
    () => this.result.value()?.data ?? INITIAL_USER_MODEL
  );

  readonly loading = computed(() => this.result.isLoading());

  readonly breadcrumbs = signal<BreadCrumbModel[]>([]);
}
