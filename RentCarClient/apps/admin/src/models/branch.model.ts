import { BaseEntityModel } from './base.entity';

export interface BranchModel extends BaseEntityModel {
  name: string;
  address: AddressValueObject;
}

export interface AddressValueObject {
  city: string;
  district: string;
  fullAddress: string;
  phoneNumber1: string;
  phoneNumber2?: string;
  email?: string;
}
