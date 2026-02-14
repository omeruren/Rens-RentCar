export interface BaseEntityModel {
  id: string;
  createdAt: string;
  createdBy: string;
  createdFullName: string;

  updatedAt?: string;
  updatedBy?: string;
  updatedFullName?: string;
}
