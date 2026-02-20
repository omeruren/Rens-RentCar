import { BaseEntityModel } from './base.entity';

export interface RoleModel extends BaseEntityModel {
  name: string;
  permissionCount: string;
  permissions: string[];
}

export const INITIAL_ROLE_MODEL: RoleModel = {
  id: '',
  name: '',
  permissionCount: '',
  permissions: [],
  isActive: true,
  createdAt: '',
  createdBy: '',
  createdFullName: '',
};
