import {
  BreadCrumbModel,
  BreadcrumbService,
} from './../../services/breadcrumb';
import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  signal,
  ViewEncapsulation,
} from '@angular/core';
import Blank from '../../components/blank/blank';
import { httpResource } from '@angular/common/http';
import { ODataModel } from '../../models/odata.model';
import { BranchModel } from '../../models/branch.model';
import { FlexiGridModule, FlexiGridService, StateModel } from 'flexi-grid';
import { NgxMaskPipe } from 'ngx-mask';
import { RouterLink } from '@angular/router';
@Component({
  imports: [Blank,RouterLink, FlexiGridModule,NgxMaskPipe],
  templateUrl: './branches.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class Branches {
  // <-- Services -->
  readonly #breadcrumb = inject(BreadcrumbService);
  readonly #grid = inject(FlexiGridService);
  /**
   *
   */
  constructor() {
    this.#breadcrumb.reset(this.breadcrumb());
  }
  readonly breadcrumb = signal<BreadCrumbModel[]>([
    {
      title: 'Branches',
      icon: 'bi-buildings',
      url: '/branches',
      isActive: true,
    },
  ]);

  readonly result = httpResource<ODataModel<BranchModel[]>>(() => {
    let endpoint = 'rent/odata/branches?$count=true';
    let part = this.#grid.getODataEndpoint(this.state());
    endpoint = endpoint + `&${part}`;
    return endpoint;
  });
  readonly data = computed(() => this.result.value()?.value ?? []);
  readonly totalCount = computed(
    () => this.result.value()?.['@count.count'] ?? 0
  );
  readonly loading = computed(() => this.result.isLoading());

  readonly state = signal<StateModel>(new StateModel());
  dataStateChange(state: StateModel) {
    this.state.set(state);
  }
}
