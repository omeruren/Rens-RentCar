import { BaseEntityModel } from './base.entity';

export interface CustomerModel extends BaseEntityModel {
  nationalId: string;
  firstName: string;
  lastName: string;
  fullName: string;
  phoneNumber: string;
  email: string;
  birthDate: string;
  drivingLicenseIssueDate: string;
  fullAddress: string;
}

export const INITIAL_CUSTOMER_MODEL: CustomerModel = {
  id: '',
  nationalId: '',
  firstName: '',
  lastName: '',
  fullName: '',
  phoneNumber: '',
  email: '',
  birthDate: '',
  drivingLicenseIssueDate: '',
  fullAddress: '',

  createdAt: '',
  createdBy: '',
  createdFullName: '',

  updatedAt: '',
  updatedBy: '',
  updatedFullName: '',

  isActive: true,
};
