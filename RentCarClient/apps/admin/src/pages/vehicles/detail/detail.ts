import { CommonModule, CurrencyPipe } from '@angular/common';
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
import { FeatureGroup } from '../create/create';
import { TrCurrencyPipe } from 'tr-currency';

@Component({
  imports: [Blank, CommonModule, TrCurrencyPipe],
  templateUrl: './detail.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class Detail {
  readonly id = signal<string>('');
  readonly bredcrumbs = signal<BreadCrumbModel[]>([]);
  readonly result = httpResource<Result<VehicleModel>>(
    () => `rent/vehicles/${this.id()}`
  );
  readonly data = computed(
    () => this.result.value()?.data ?? INITIAL_VEHICLE_MODEL
  );
  readonly loading = computed(() => this.result.isLoading());
  readonly pageTitle = signal<string>('Vehicle Detail');

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
        {
          key: 'Touch Screen',
          label: 'Touch Screen',
          icon: 'bi-tablet',
        },
        { key: 'USB Connection', label: 'USB Connection', icon: 'bi-usb' },
        {
          key: 'Premium Audio System',
          label: 'Premium Audio System',
          icon: 'bi-music-note-beamed',
        },
        { key: 'Apple CarPlay', label: 'Apple CarPlay', icon: 'bi-phone' },
      ],
    },
  ];

  readonly #activated = inject(ActivatedRoute);
  readonly #breadcrumb = inject(BreadcrumbService);

  constructor() {
    this.#activated.params.subscribe((res) => {
      this.id.set(res['id']);
    });

    effect(() => {
      const breadCrumbs: BreadCrumbModel[] = [
        {
          title: 'Vehicles',
          icon: 'bi-car-front',
          url: '/vehicles',
        },
      ];

      if (this.data()) {
        this.bredcrumbs.set(breadCrumbs);
        this.bredcrumbs.update((prev) => [
          ...prev,
          {
            title: this.data().brand + ' ' + this.data().model,
            icon: 'bi-zoom-in',
            url: `/vehicles/detail/${this.id()}`,
            isActive: true,
          },
        ]);
        this.#breadcrumb.reset(this.bredcrumbs());
      }
    });
  }

  showImageUrl() {
    if (this.data().imageUrl) {
      return `https://localhost:7203/images/${this.data().imageUrl}`;
    } else {
      return '/no-noimage.png';
    }
  }

  isFeatureSelected(feature: string): boolean {
    return this.data().features.includes(feature);
  }
}
