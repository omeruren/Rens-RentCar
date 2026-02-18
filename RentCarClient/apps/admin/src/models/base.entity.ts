export interface BaseEntityModel {
  id: string;
  createdAt: string;
  createdBy: string;
  createdFullName: string;
  isActive: boolean;

  updatedAt?: string;
  updatedBy?: string;
  updatedFullName?: string;
}
