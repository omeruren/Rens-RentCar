import { httpResource } from '@angular/common/http';
import {
  AfterViewInit,
  ChangeDetectionStrategy,
  Component,
  computed,
  contentChild,
  contentChildren,
  inject,
  input,
  signal,
  TemplateRef,
  ViewEncapsulation,
} from '@angular/core';
import {
  FlexiGridColumnComponent,
  FlexiGridModule,
  FlexiGridService,
  StateModel,
} from 'flexi-grid';
import { ODataModel } from '../../models/odata.model';
import { RouterLink } from '@angular/router';
import { FlexiToastService } from 'flexi-toast';
import { HttpService } from '../../services/http';
import { BreadCrumbModel, BreadcrumbService } from '../../services/breadcrumb';
import { NgTemplateOutlet } from '@angular/common';
import { Common } from '../../services/common';

export interface btnOptions {
  url: string;
  permission: string;
}

@Component({
  selector: 'app-grid',
  imports: [FlexiGridModule, RouterLink, NgTemplateOutlet],
  templateUrl: './grid.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class Grid implements AfterViewInit {
  // <-- Services -->
  readonly #grid = inject(FlexiGridService);
  readonly #toast = inject(FlexiToastService);
  readonly #http = inject(HttpService);
  readonly #breadcrumb = inject(BreadcrumbService);
  readonly #common = inject(Common);

  /**
   *
   */
  ngAfterViewInit(): void {
    this.#breadcrumb.reset(this.breadcrumbs());
  }
  readonly pageTitle = input.required<string>();
  readonly showAuditInfos = input<boolean>(true);
  readonly breadcrumbs = input.required<BreadCrumbModel[]>();
  readonly commandColumnWidth = input<string>('150px');
  readonly showIndex = input<boolean>(true);
  readonly captionTitle = input.required<string>();
  readonly endpointEntityName = input.required<string>();
  readonly permissionEntityName = input.required<string>();
  readonly endpoint = input<string>('');
  readonly showIsActive = input<boolean>(true);

  permissionView = signal<string>('');
  permissionCreate = signal<string>('');
  permissionEdit = signal<string>('');
  permissionDetails = signal<string>('');
  permissionDelete = signal<string>('');

  ngOnInit() {
    const base = this.permissionEntityName();

    this.permissionView.set(base + ':view');
    this.permissionCreate.set(base + ':create');
    this.permissionEdit.set(base + ':edit');
    this.permissionDetails.set(base + ':view');
    this.permissionDelete.set(base + ':delete');
    console.log(this.result.value()?.value);
  }

  readonly columns = contentChildren(FlexiGridColumnComponent, {
    descendants: true,
  });
  readonly commandTemplateRef =
    contentChild<TemplateRef<any>>('commandTemplate');
  readonly columnCommandTemplateRef = contentChild<TemplateRef<any>>(
    'columnCommandTemplate'
  );
  readonly state = signal<StateModel>(new StateModel());

  readonly result = httpResource<ODataModel<any>>(() => {
    let endpoint = '';
    if (this.endpoint().includes('?')) {
      endpoint += `${this.endpoint()}&$count=true`;
    } else {
      endpoint = `rent/odata/${this.endpointEntityName()}?$count=true`;
    }
    const part = this.#grid.getODataEndpoint(this.state());
    endpoint += `&${part}`;
    return endpoint;
  });

  readonly data = computed(() => this.result.value()?.value ?? []);
  readonly totalCount = computed(
    () => this.result.value()?.['@odata.count'] ?? 0
  );
  readonly loading = computed(() => this.result.isLoading());

  dataStateChange(state: StateModel) {
    this.state.set(state);
  }

  delete(id: string) {
    this.#toast.showSwal(
      'Remove',
      'Are you sure ? (you can not undo this)',
      'Remove',
      () => {
        this.#http.delete<string>(
          `rent/${this.endpointEntityName()}/${id}`,
          (res) => {
            this.#toast.showToast('Success', res, 'info');
            this.result.reload();
          }
        );
      },
      'Cancel'
    );
  }

  checkPermission(permission: string) {
    return this.#common.checkPermission(permission);
  }
}
