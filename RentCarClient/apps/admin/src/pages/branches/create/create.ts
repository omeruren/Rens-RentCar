import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  signal,
  ViewEncapsulation,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import Blank from 'apps/admin/src/components/blank/blank';
import Loading from 'apps/admin/src/components/loading/loading';
import {
  BreadCrumbModel,
  BreadcrumbService,
} from 'apps/admin/src/services/breadcrumb';
import { FormValidateDirective } from 'form-validate-angular';

@Component({
  imports: [Blank, FormsModule, FormValidateDirective, Loading],
  templateUrl: './create.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class Create {
  //  <-- Services -->
  readonly #breadcrumb = inject(BreadcrumbService);
  readonly #activated = inject(ActivatedRoute);

readonly loading = signal<boolean>(false);

  readonly pageTitle = computed(() =>
    this.id() ? 'Edit Branch' : 'Add Branch'
  );
  pageIcon = computed(() => (this.id() ? ' bi-pen' : ' bi-plus '));
  /**
   *
   */
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
      }
      this.#breadcrumb.reset(this.breadcrumbs());
    });
  }
  readonly id = signal<string | undefined>(undefined);
  readonly breadcrumbs = signal<BreadCrumbModel[]>([
    {
      title: 'Branches',
      icon: 'bi-buildings',
      url: '/branches',
    },
  ]);
}
