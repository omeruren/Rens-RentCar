import { BaseEntityModel } from './base.entity';

export interface ExtraModel extends BaseEntityModel {
  name: string;
  price: number;
  description: string;
}

export const INITIAL_EXTRA_MODEL: ExtraModel = {
  id: '',
  name: '',
  price: 0,
  description: '',

  createdAt: '',
  createdBy: '',
  createdFullName: '',

  updatedAt: '',
  updatedBy: '',
  updatedFullName: '',
  isActive: true,
};
