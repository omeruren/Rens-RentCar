import { BaseEntityModel } from './base.entity';

export interface UserModel extends BaseEntityModel {
  firstName: string;
  lastName: string;
  fullName: string;
  userName: string;
  email: string;
  branchId: string | null;
  branchName: string;
  roleId: string;
  roleName: string;
}

export const INITIAL_USER_MODEL: UserModel = {
  id: '',
  firstName: '',
  lastName: '',
  fullName: '',
  userName: '',
  email: '',
  branchId: null,
  branchName: '',
  roleId: '',
  roleName: '',
  isActive: true,
  createdAt: '',
  createdBy: '',
  createdFullName: '',
};
