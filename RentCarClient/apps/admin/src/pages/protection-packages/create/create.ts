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
  ProtectionPackageModel,
  INITIAL_PROTECTION_PACKAGE_MODEL,
} from 'apps/admin/src/models/protection.package.model';
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
  imports: [
    Blank,
    FormsModule,
    FormValidateDirective,
    NgxMaskDirective
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
            url: '/protection-packages/add',
            isActive: true,
          },
        ]);
        this.#breadcrumb.reset(this.breadcrumbs());
      }
    });

    // Initialize coveragesText when data changes
    effect(() => {
      if (this.data().coverages && this.data().coverages.length > 0) {
        this.coveragesText.set(this.data().coverages.join(', '));
      }
    });
  }

  readonly result = resource({
    params: () => this.id(),
    loader: async () => {
      var res = await lastValueFrom(
        this.#http.getResource<ProtectionPackageModel>(`rent/protection-packages/${this.id()}`)
      );
      this.breadcrumbs.update((prev) => [
        ...prev,
        {
          title: res.data!.name,
          icon: 'bi-pen',
          url: `/protection-packages/edit/${this.id()}`,
          isActive: true,
        },
      ]);
      this.#breadcrumb.reset(this.breadcrumbs());

      return res.data;
    },
  });

  readonly loading = linkedSignal(() => this.result.isLoading());

  readonly data = linkedSignal(
    () => this.result.value() ?? { ...INITIAL_PROTECTION_PACKAGE_MODEL }
  );

  readonly coveragesText = signal<string>('');

  readonly pageTitle = computed(() =>
    this.id() ? 'Edit Protection' : 'Add Protection'
  );
  readonly pageIcon = computed(() => (this.id() ? ' bi-pen' : ' bi-plus '));
  readonly id = signal<string | undefined>(undefined);

  readonly breadcrumbs = signal<BreadCrumbModel[]>([
    {
      title: 'Protection Packages',
      icon: 'bi-shield',
      url: '/protection-packages',
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

    // Convert coveragesText to coverages array
    const coveragesArray = this.coveragesText()
      .split(',')
      .map(c => c.trim())
      .filter(c => c.length > 0);

    this.data.update((prev) => ({
      ...prev,
      coverages: coveragesArray,
    }));

    if (!this.id()) {
      this.#http.post<string>(
        'rent/protection-packages',
        this.data(),
        (res) => {
          this.loading.set(false);
          this.#toast.showToast('Success', res, 'success');
          this.#router.navigateByUrl('/protection-packages');
        },
        () => this.loading.set(false)
      );
    } else {
      this.#http.put<string>(
        'rent/protection-packages',
        this.data(),
        (res) => {
          this.loading.set(false);
          this.#toast.showToast('Success', res, 'info');
          this.#router.navigateByUrl('/protection-packages');
        },
        () => this.loading.set(false)
      );
    }
  }
}
