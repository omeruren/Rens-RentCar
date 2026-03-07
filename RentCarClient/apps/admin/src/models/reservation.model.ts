import { BaseEntityModel } from './base.entity';

export interface ReservationModel extends BaseEntityModel {
  customerId: string;

  customerFullName: string;
  customerNationalId: string;
  customerPhoneNumber: string;
  customerEmail: string;
  customerFullAddress: string;

  pickUpLocationId?: string;
  pickUpName: string;
  pickUpFullAddress: string;
  pickUpPhoneNumber: string;

  pickUpDate: string;
  pickUpTime: string;
  pickupDateTime: string;

  deliveryDate: string;
  deliveryTime: string;
  deliveryDateTime: string;

  vehicleId: string;
  vehicleDailyPrice: number;

  vehicleBrand: string;
  vehicleModel: string;
  vehicleModelYear: number;
  vehicleColor: string;
  vehicleCategoryName: string;
  vehicleFuelConsumption: number;
  vehicleSeatCount: number;
  vehicleTractionType: string;
  vehicleKilometer: number;
  vehicleImageUrl: string;

  protectionPackageId: string;
  protectionPackagePrice: number;
  protectionPackageName: string;
  reservationExtras: {
    extraId: string;
    extraName: string;
    price: number;
  }[];
  note: string;
  total: number;
  status: string;
  totalDay: number;
}

export const INITIAL_RESERVATION_MODEL: ReservationModel = {
  customerId: '',
  customerFullName: '',
  customerNationalId: '',
  customerPhoneNumber: '',
  customerEmail: '',
  customerFullAddress: '',

  pickUpName: '',
  pickUpFullAddress: '',
  pickUpPhoneNumber: '',

  pickUpDate: '',
  pickUpTime: '09:00',
  pickupDateTime: '',

  deliveryDate: '',
  deliveryTime: '09:00',
  deliveryDateTime: '',

  vehicleId: '',
  vehicleDailyPrice: 0,

  vehicleBrand: '',
  vehicleModel: '',
  vehicleModelYear: 0,
  vehicleColor: '',
  vehicleCategoryName: '',
  vehicleFuelConsumption: 0,
  vehicleSeatCount: 0,
  vehicleTractionType: '',
  vehicleKilometer: 0,
  vehicleImageUrl: '',
  protectionPackageId: '',
  protectionPackagePrice: 0,
  protectionPackageName: '',
  reservationExtras: [],
  note: '',
  total: 0,
  status: '',
  totalDay: 0,

  id: '',
  isActive: true,
  createdAt: '',
  createdBy: '',
  createdFullName: '',
};
