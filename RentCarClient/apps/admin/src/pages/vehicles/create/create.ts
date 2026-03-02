import { DatePipe, NgClass } from '@angular/common';
import { httpResource } from '@angular/common/http';
import {
  ChangeDetectionStrategy,
  Component,
  computed,
  effect,
  ElementRef,
  inject,
  linkedSignal,
  resource,
  signal,
  viewChild,
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
export interface FeatureGroup {
  group: string;
  features: { key: string; label: string; icon: string }[];
}

@Component({
  imports: [
    Blank,
    FormsModule,
    FormValidateDirective,
    NgClass,
    NgxMaskDirective,
    FlexiSelectModule,
  ],
  templateUrl: './create.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [DatePipe],
})
export default class CreateVehicle {
  //  <-- Services -->
  readonly #breadcrumb = inject(BreadcrumbService);
  readonly #activated = inject(ActivatedRoute);
  readonly #http = inject(HttpService);
  readonly #toast = inject(FlexiToastService);
  readonly #router = inject(Router);
  readonly #date = inject(DatePipe);

  readonly brandList = [
    'Toyota',
    'Renault',
    'Volkswagen',
    'Ford',
    'Fiat',
    'Hyundai',
    'Peugeot',
    'Opel',
    'Honda',
    'BMW',
  ];
  readonly modelYearList = Array.from({ length: 21 }, (_, i) => 2010 + i); // 2010-2030 range
  readonly colorList = [
    'White',
    'Black',
    'Gray',
    'Red',
    'Blue',
    'Green',
    'Yellow',
    'Orange',
    'Brown',
    'Purple',
  ];
  readonly fuelTypeList = ['Gasoline', 'Diesel', 'LPG', 'Electric', 'Hybrid'];

  readonly transmissionList = ['Manual', 'Automatic', 'CVT'];

  readonly seatCountList = [
    { value: 2, label: '2 Seats' },
    { value: 4, label: '4 Seats' },
    { value: 5, label: '5 Seats' },
    { value: 7, label: '7 Seats' },
    { value: 8, label: '8 Seats' },
    { value: 9, label: '9+ Seats' },
  ];

  readonly featureGroups: FeatureGroup[] = [
    {
      group: 'Safety Features',
      features: [
        { key: 'Airbag', label: 'Airbag', icon: 'bi-shield' },
        { key: 'ABS', label: 'ABS', icon: 'bi-shield-exclamation' },
        { key: 'ESP', label: 'ESP', icon: 'bi-shield-check' },
        { key: 'Alarm System', label: 'Alarm System', icon: 'bi-bell' },
      ],
    },
    {
      group: 'Driving Assistance',
      features: [
        { key: 'GPS Navigation', label: 'GPS Navigation', icon: 'bi-geo-alt' },
        {
          key: 'Parking Sensor',
          label: 'Parking Sensor',
          icon: 'bi-broadcast-pin',
        },
        {
          key: 'Rear View Camera',
          label: 'Rear View Camera',
          icon: 'bi-camera-video',
        },
        {
          key: 'Cruise Control',
          label: 'Cruise Control',
          icon: 'bi-speedometer2',
        },
      ],
    },
    {
      group: 'Comfort Features',
      features: [
        { key: 'Air Conditioning', label: 'Air Conditioning', icon: 'bi-snow' },
        {
          key: 'Heated Seats',
          label: 'Heated Seats',
          icon: 'bi-thermometer-half',
        },
        { key: 'Sunroof', label: 'Sunroof', icon: 'bi-brightness-high' },
        { key: 'Bluetooth', label: 'Bluetooth', icon: 'bi-bluetooth' },
      ],
    },
    {
      group: 'Multimedia',
      features: [
        { key: 'Touchscreen', label: 'Touchscreen', icon: 'bi-tablet' },
        { key: 'USB Connection', label: 'USB Connection', icon: 'bi-usb' },
        {
          key: 'Premium Sound System',
          label: 'Premium Sound System',
          icon: 'bi-music-note-beamed',
        },
        { key: 'Apple CarPlay', label: 'Apple CarPlay', icon: 'bi-phone' },
      ],
    },
  ];

  readonly insuranceTypeList = signal<string[]>([
    'comprehensive insurance',
    'Traffic',
    'comprehensive insurance & Traffic',
    'None',
    'Full',
    'Partial',
    'comprehensive insurance & Insurance',
    'Insurance',
  ]);

  readonly tractionTypeList = [
    'Front-Wheel Drive',
    'Rear-Wheel Drive',
    '4x4',
    'AWD',
    'Other',
  ];

  readonly tireStatusList = [
    'New',
    'Good',
    'Average',
    'Weak',
    'Needs Replacement',
  ];

  readonly generalStatusList = [
    'Excellent',
    'Good',
    'Average',
    'Maintenance Required',
    'Damaged',
  ];

  readonly id = signal<string | undefined>(undefined);
  readonly breadcrumbs = signal<BreadCrumbModel[]>([
    {
      title: 'Vehicles',
      icon: 'bi-car-front',
      url: '/vehicles',
    },
  ]);
  readonly pageTitle = computed(() =>
    this.id() ? 'Update Vehicle' : 'Add Vehicle'
  );
  readonly pageIcon = computed(() => (this.id() ? 'bi-pen' : 'bi-plus'));
  readonly btnName = computed(() => (this.id() ? 'Update' : 'Save'));
  readonly result = resource({
    params: () => this.id(),
    loader: async () => {
      if (!this.id()) return null;
      const res = await lastValueFrom(
        this.#http.getResource<VehicleModel>(`rent/vehicles/${this.id()}`)
      );
      this.breadcrumbs.update((prev) => [
        ...prev,
        {
          title: res.data!.brand + ' ' + res.data!.model,
          icon: 'bi-pen',
          url: `/vehicles/edit/${this.id()}`,
          isActive: true,
        },
      ]);
      this.#breadcrumb.reset(this.breadcrumbs());
      return res.data;
    },
  });
  readonly data = linkedSignal(
    () => this.result.value() ?? { ...INITIAL_VEHICLE_MODEL }
  );
  readonly loading = linkedSignal(() => this.result.isLoading());
  readonly categoryResource = httpResource<ODataModel<CategoryModel>>(
    () => 'rent/odata/categories'
  );
  readonly categories = computed(
    () => this.categoryResource.value()?.value ?? []
  );
  readonly categoryLoading = computed(() => this.categoryResource.isLoading());

  readonly branchResource = httpResource<ODataModel<BranchModel>>(
    () => 'rent/odata/branches'
  );
  readonly branches = computed(() => this.branchResource.value()?.value ?? []);
  readonly branchLoading = computed(() => this.branchResource.isLoading());
  featuresInput = '';
  readonly file = signal<any | undefined>(undefined);
  readonly fileData = signal<string | undefined>(undefined);

  readonly fileInput =
    viewChild.required<ElementRef<HTMLInputElement>>('fileInput');
  dragOver = false;

  constructor() {
    this.#activated.params.subscribe((res) => {
      if (res['id']) {
        this.id.set(res['id']);
      } else {
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

        const date = this.#date.transform(new Date(), 'yyyy-MM-dd')!;
        this.data.update((prev) => ({
          ...prev,
          cascoEndDate: date,
          inspectionDate: date,
          insuranceEndDate: date,
          lastMaintenanceDate: date,
        }));
      }
    });
  }

  save(form: NgForm) {
    if (!form.valid) return;
    this.loading.set(true);

    // Create FormData
    const formData = new FormData();
    const d = this.data();

    // Add all fields
    formData.append('Brand', d.brand);
    formData.append('Model', d.model);
    formData.append('ModelYear', d.modelYear.toString());
    formData.append('Color', d.color);
    formData.append('Plate', d.plate);
    formData.append('CategoryId', d.categoryId);
    formData.append('BranchId', d.branchId);
    formData.append('VinNumber', d.vinNumber);
    formData.append('EngineNumber', d.engineNumber);
    formData.append('Description', d.description);
    formData.append('FuelType', d.fuelType);
    formData.append('Transmission', d.transmission);
    formData.append('EngineVolume', d.engineVolume?.toString() ?? '0');
    formData.append('EnginePower', d.enginePower?.toString() ?? '0');
    formData.append('TractionType', d.tractionType);
    formData.append('FuelConsumption', d.fuelConsumption?.toString() ?? '0');
    formData.append('SeatCount', d.seatCount?.toString() ?? '0');
    formData.append('Kilometer', d.kilometer?.toString() ?? '0');
    formData.append('DailyPrice', d.dailyPrice?.toString() ?? '0');
    formData.append(
      'WeeklyDiscountRate',
      d.weeklyDiscountRate?.toString() ?? '0'
    );
    formData.append(
      'MonthlyDiscountRate',
      d.monthlyDiscountRate?.toString() ?? '0'
    );
    formData.append('InsuranceType', d.insuranceType);
    formData.append('LastMaintenanceDate', d.lastMaintenanceDate);
    formData.append(
      'LastMaintenanceKm',
      d.lastMaintenanceKm?.toString() ?? '0'
    );
    formData.append(
      'NextMaintenanceKm',
      d.nextMaintenanceKm?.toString() ?? '0'
    );
    formData.append('InspectionDate', d.inspectionDate);
    formData.append('InsuranceEndDate', d.insuranceEndDate);
    formData.append('CascoEndDate', d.cascoEndDate);
    formData.append('TireStatus', d.tireStatus);
    formData.append('GeneralStatus', d.generalStatus);
    formData.append('IsActive', d.isActive ? 'true' : 'false');

    // Features (array)
    if (d.features && d.features.length > 0) {
      d.features.forEach((f) => formData.append('Features', f));
    }

    // Image file (selected via fileInput)
    if (this.file()) {
      formData.append('File', this.file(), this.file().name);
    }

    // Add Id for update
    if (this.id()) {
      formData.append('Id', this.id()!);
    }

    const endpoint = this.id() ? 'rent/vehicles' : 'rent/vehicles';
    const method = this.id() ? this.#http.put<string> : this.#http.post<string>;

    method.call(
      this.#http,
      endpoint,
      formData,
      (res) => {
        this.#toast.showToast('Success', res, this.id() ? 'info' : 'success');
        this.#router.navigateByUrl('/vehicles');
        this.loading.set(false);
      },
      () => this.loading.set(false)
    );
  }

  changeStatus(status: boolean) {
    this.data.update((prev) => ({
      ...prev,
      isActive: status,
    }));
  }

  addFeature() {
    if (this.featuresInput.trim()) {
      this.data.update((prev) => ({
        ...prev,
        features: [...prev.features, this.featuresInput.trim()],
      }));
      this.featuresInput = '';
    }
  }

  removeFeature(index: number) {
    this.data.update((prev) => ({
      ...prev,
      features: prev.features.filter((_, i) => i !== index),
    }));
  }

  triggerFileInput() {
    const fileInput = this.fileInput();
    fileInput.nativeElement.value = ''; // Reset so the same file can be selected again
    fileInput.nativeElement.click();
  }

  onImageChange(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      this.file.set(file);
      const reader = new FileReader();
      reader.onload = () => {
        this.fileData.set(reader.result as string);
        this.data.update((prev) => ({
          ...prev,
          imageUrl: '',
        }));
      };
      reader.readAsDataURL(file);
      input.value = ''; // Reset so the same file can be selected again
    }
  }

  onImageDragOver(event: DragEvent) {
    event.preventDefault();
    this.dragOver = true;
  }

  onImageDragLeave(event: DragEvent) {
    event.preventDefault();
    this.dragOver = false;
  }

  onImageDrop(event: DragEvent) {
    event.preventDefault();
    this.dragOver = false;
    if (event.dataTransfer && event.dataTransfer.files.length > 0) {
      const file = event.dataTransfer.files[0];
      this.file.set(file);
      const reader = new FileReader();
      reader.onload = () => {
        this.fileData.set(reader.result as string);
        this.data.update((prev) => ({
          ...prev,
          imageUrl: '',
        }));
      };
      reader.readAsDataURL(file);
      const fileInput = this.fileInput();
      if (fileInput) fileInput.nativeElement.value = '';
    }
  }

  // Manages feature selection
  toggleFeature(feature: string) {
    const features = this.data().features;
    if (features.includes(feature)) {
      this.data.update((prev) => ({
        ...prev,
        features: prev.features.filter((f) => f !== feature),
      }));
    } else {
      this.data.update((prev) => ({
        ...prev,
        features: [...prev.features, feature],
      }));
    }
  }

  // Check if feature is selected
  isFeatureSelected(feature: string): boolean {
    return this.data().features.includes(feature);
  }
  showImageUrl() {
    if (this.fileData()) {
      return this.fileData();
    } else if (this.data().imageUrl) {
      return `https://localhost:7203/images/${this.data().imageUrl}`;
    } else {
      return '/no-noimage.png';
    }
  }
}
