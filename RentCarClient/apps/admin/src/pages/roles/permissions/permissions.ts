import { httpResource } from '@angular/common/http';
import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  signal,
  ViewEncapsulation,
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Result } from 'apps/admin/src/models/result.model';
import {
  BreadCrumbModel,
  BreadcrumbService,
} from 'apps/admin/src/services/breadcrumb';
import {
  FlexiTreeNode,
  FlexiTreeviewComponent,
  FlexiTreeviewService,
} from 'flexi-treeview';

@Component({
  imports: [FlexiTreeviewComponent],
  templateUrl: './permissions.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class Permissions {
  // <-- Services -->
  readonly #activated = inject(ActivatedRoute);
  readonly #breadcrumb = inject(BreadcrumbService);
  readonly #treeview = inject(FlexiTreeviewService);
  constructor() {
    this.#activated.params.subscribe((res) => {
      this.id.set(res['id']);
      this.#breadcrumb.reset(this.breadcrumbs());
    });
  }
  readonly id = signal<string>('');

  readonly breadcrumbs = computed<BreadCrumbModel[]>(() => [
    {
      title: 'Roles',
      icon: 'bi-person-lock',
      url: '/roles',
    },
    {
      title: 'Permissions',
      icon: '',
      url: `/roles/permissions/${this.id()}`,
      isActive: true,
    },
  ]);

  readonly result = httpResource<Result<string[]>>(() => `rent/permissions`);
  readonly data = computed(() => {
    const data = this.result.value()?.data ?? [];
    const nodes = data.map((val) => {
      var parts = val.split(':');
      var data = { id: val, code: parts[0], name: parts[1] };
      return data;
    });
    const treeNodes: FlexiTreeNode[] = this.#treeview.convertToTreeNodes(
      nodes,
      'id',
      'code',
      'name'
    );
    return treeNodes;
  });
  readonly loading = computed(() => this.result.isLoading());

  onSelected(event: any) {
    console.log(event);
  }
}
