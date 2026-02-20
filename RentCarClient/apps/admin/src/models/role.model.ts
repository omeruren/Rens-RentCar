import { BaseEntityModel } from './base.entity';

export interface RoleModel extends BaseEntityModel {
  name: string;
}

export const INITIAL_ROLE_MODEL: RoleModel = {
  id: '',
  name: '',

  isActive: true,
  createdAt: '',
  createdBy: '',
  createdFullName: '',
};
