import { BaseEntityModel } from './base.entity';

export interface ProtectionPackageModel extends BaseEntityModel {
    name: string;
    price: number;
    isRecommended: boolean;
    coverages: string[];
}

export const  INITIAL_PROTECTION_PACKAGE_MODEL: ProtectionPackageModel = {
    id: '',
    name: '',
    price: 0,
    isRecommended: false,
    coverages: [],
    createdAt: '',
    createdBy: '',
    updatedAt: '',
    updatedBy: '',
    isActive: true,
    createdFullName: '',
    updatedFullName: '',
};
