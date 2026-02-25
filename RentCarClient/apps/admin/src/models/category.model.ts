import { BaseEntityModel } from './base.entity';

export interface CategoryModel extends BaseEntityModel {
  name: string;
}

export const INITIAL_CATEGORY_MODEL: CategoryModel = {
  id: '',
  name: '',

  createdAt: '',
  createdBy: '',
  createdFullName: '',

  updatedAt: '',
  updatedBy: '',
  updatedFullName: '',
  isActive: true,
};
