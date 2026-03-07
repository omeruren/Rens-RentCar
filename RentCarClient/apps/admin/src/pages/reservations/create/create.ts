import { DatePipe, NgClass, NgTemplateOutlet } from '@angular/common';
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
import Grid from 'apps/admin/src/components/grid/grid';
import { BranchModel } from 'apps/admin/src/models/branch.model';
import {
  CustomerModel,
  INITIAL_CUSTOMER_MODEL,
} from 'apps/admin/src/models/customer.model';
import { ODataModel } from 'apps/admin/src/models/odata.model';
import {
  INITIAL_RESERVATION_MODEL,
  ReservationModel,
} from 'apps/admin/src/models/reservation.model';
import {
  BreadcrumbService,
  BreadCrumbModel,
} from 'apps/admin/src/services/breadcrumb';
import { Common } from 'apps/admin/src/services/common';
import { HttpService } from 'apps/admin/src/services/http';
import { FlexiGridModule, FlexiGridService, StateModel } from 'flexi-grid';
import { FlexiPopupModule } from 'flexi-popup';
import { FlexiSelectModule } from 'flexi-select';
import { FlexiToastService } from 'flexi-toast';
import { FormValidateDirective } from 'form-validate-angular';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';
import { lastValueFrom } from 'rxjs';

@Component({
  imports: [
    Blank,
    FormsModule,
    FormValidateDirective,
    FlexiGridModule,
    FlexiPopupModule,
    NgClass,
    NgTemplateOutlet,
    NgxMaskPipe,
    DatePipe,
    FlexiSelectModule,
  ],
  templateUrl: './create.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [DatePipe],
})
export default class Create {
  //  <-- Services -->
  readonly #breadcrumb = inject(BreadcrumbService);
  readonly #activated = inject(ActivatedRoute);
  readonly #router = inject(Router);
  readonly #http = inject(HttpService);
  readonly #common = inject(Common);
  readonly #date = inject(DatePipe);
  readonly #toast = inject(FlexiToastService);
  readonly #flexiGrid = inject(FlexiGridService);

  constructor() {
    this.#activated.params.subscribe((res) => {
      if (res['id']) this.id.set(res['id']);
      else {
        this.breadcrumbs.update((prev) => [
          ...prev,
          {
            title: 'Add',
            icon: 'bi-plus',
            url: '/reservations/add',
            isActive: true,
          },
        ]);
        this.#breadcrumb.reset(this.breadcrumbs());
        const date = this.#date.transform('01.01.2000', 'yyyy-MM-dd')!;
        this.customerPopUpData.update((prev) => ({
          ...prev,
          birthDate: date,
          drivingLicenseIssueDate: date,
        }));
        const today = this.#date.transform(new Date(), 'yyyy-MM-dd')!;
        const tomorrow = this.#date.transform(
          new Date().setDate(new Date().getDate() + 1),
          'yyyy-MM-dd'
        )!;
        this.data.update((prev) => ({
          ...prev,
          pickUpDate: today,
          deliveryDate: tomorrow,
        }));
      }
    });
    this.calculateDayDifferance();
  }

  readonly timeData = signal<string[]>([
    '09:00',
    '09:30',
    '10:00',
    '10:30',
    '11:00',
    '11:30',
    '12:00',
    '12:30',
    '13:00',
    '13:30',
    '14:00',
    '14:30',
    '15:00',
    '15:30',
    '16:00',
    '16:30',
    '17:00',
    '17:30',
    '18:00',
    '18:30',
    '19:00',
    '19:30',
    '20:00',
    '20:30',
    '21:00',
    '21:30',
    '22:00',
    '22:30',
    '23:00',
    '23:30',
    '00:00',
  ]);

  readonly branchName = linkedSignal(() => this.#common.token().branch);
  readonly isAdmin = computed(() => this.#common.token().role === 'sys_admin');

  readonly isCustomerPopUpLoading = signal<boolean>(false);
  readonly customerPopUpData = signal<CustomerModel>({
    ...INITIAL_CUSTOMER_MODEL,
  });
  isCustomerPopUpVisible = false;

  readonly customerState = signal<StateModel>(new StateModel());

  readonly customerResult = httpResource<ODataModel<CustomerModel>>(() => {
    let endpoint = 'rent/odata/customers?count=true&';
    const part = this.#flexiGrid.getODataEndpoint(this.customerState());
    endpoint += part;
    return endpoint;
  });
  readonly customerData = computed(
    () => this.customerResult.value()?.value ?? []
  );

  readonly customerTotal = computed(
    () => this.customerResult.value()?.['@odata.count'] ?? 0
  );
  readonly customerLoading = computed(() => this.customerResult.isLoading());

  readonly selectedCustomer = signal<CustomerModel | undefined>(undefined);
  readonly result = resource({
    params: () => this.id(),
    loader: async () => {
      var res = await lastValueFrom(
        this.#http.getResource<ReservationModel>(
          `rent/reservations/${this.id()}`
        )
      );
      this.breadcrumbs.update((prev) => [
        ...prev,
        {
          title: res.data!.customerFullName,
          icon: 'bi-pen',
          url: `/reservations/edit/${this.id()}`,
          isActive: true,
        },
      ]);
      this.#breadcrumb.reset(this.breadcrumbs());

      return res.data;
    },
  });

  readonly branchState = signal<StateModel>(new StateModel());
  readonly branchResult = httpResource<ODataModel<BranchModel>>(
    () => 'rent/odata/branches'
  );
  readonly branchData = computed(() => this.branchResult.value()?.value ?? []);
  readonly branchLoading = computed(() => this.branchResult.isLoading());

  readonly loading = linkedSignal(() => this.result.isLoading());

  readonly data = linkedSignal(
    () => this.result.value() ?? { ...INITIAL_RESERVATION_MODEL }
  );

  readonly pageTitle = computed(() =>
    this.id() ? 'Edit Reservation' : 'Add Reservation'
  );
  readonly pageIcon = computed(() => (this.id() ? ' bi-pen' : ' bi-plus '));
  readonly id = signal<string | undefined>(undefined);

  readonly breadcrumbs = signal<BreadCrumbModel[]>([
    {
      title: 'Reservations',
      icon: 'bi-calendar-check',
      url: '/reservations',
    },
  ]);

  save(form: NgForm) {
    if (!form.valid) return;
    this.loading.set(true);

    if (!this.id()) {
      this.#http.post<string>(
        'rent/reservations',
        this.data(),
        (res) => {
          this.loading.set(false);
          this.#toast.showToast('Success', res, 'success');
          this.#router.navigateByUrl('/reservations');
        },
        () => this.loading.set(false)
      );
    } else {
      this.#http.put<string>(
        'rent/reservations',
        this.data(),
        (res) => {
          this.loading.set(false);
          this.#toast.showToast('Success', res, 'info');
          this.#router.navigateByUrl('/reservations');
        },
        () => this.loading.set(false)
      );
    }
  }

  saveCustomer(form: NgForm) {
    if (!form.valid) return;
    this.loading.set(true);

    this.#http.post<string>(
      'rent/customers',
      this.customerPopUpData(),
      (res) => {
        this.loading.set(false);
        this.#toast.showToast('Success', res, 'success');
        // this.#router.navigateByUrl('/customers');
      },
      () => this.loading.set(false)
    );
  }

  customerDataStateChange(state: StateModel) {
    this.customerState.set(state);
  }

  selectCustomer(item: CustomerModel) {
    this.selectedCustomer.set(item);
    this.data.update((prev) => ({ ...prev, customerId: item.id }));
  }

  clearCustomer() {
    this.selectedCustomer.set(undefined);
    this.data.update((prev) => ({ ...prev, customerId: '' }));
  }

  calculateDayDifferance() {
    const pickUpDateTime = new Date(
      `${this.data().pickUpDate}T${this.data().pickUpTime}`
    );
    const deliveryDateTime = new Date(
      `${this.data().deliveryDate}T${this.data().deliveryTime}`
    );

    const diffMs = deliveryDateTime.getTime() - pickUpDateTime.getTime();

    if (diffMs <= 0) {
      this.data.update((prev) => ({ ...prev, totalDay: 0 }));
      return;
    }

    const oneDayMs = 24 * 60 * 60 * 1000;

    const fullDays = Math.floor(diffMs / oneDayMs);
    const remainder = diffMs % oneDayMs;
    const totalDay = remainder > 0 ? fullDays + 1 : fullDays;

    this.data.update((prev) => ({ ...prev, totalDay: totalDay }));
  }

  setLocation(id: any) {
    const branch = this.branchData().find((i) => i.id == id)!;
    this.branchName.set(branch.name);
  }
}
