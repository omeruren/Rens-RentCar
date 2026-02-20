import { BaseEntityModel } from './base.entity';

export interface BranchModel extends BaseEntityModel {
  name: string;
  city: string;
  district: string;
  fullAddress: string;
  phoneNumber1: string;
  phoneNumber2?: string;
  email?: string;
}

export interface AddressValueObject {
  city: string;
  district: string;
  fullAddress: string;
  phoneNumber1: string;
  phoneNumber2?: string;
  email?: string;
}

export const INITIAL_BRANCH_MODEL: BranchModel = {
  id: '',
  name: '',

  city: '',
  district: '',
  fullAddress: '',
  phoneNumber1: '',
  phoneNumber2: '',
  email: '',

  isActive: true,
  createdAt: '',
  createdBy: '',
  createdFullName: '',
};
