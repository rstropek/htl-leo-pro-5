export enum PaymentType {
  Cash,
  BankTransfer,
  CreditCard,
  DebitCard
}

export interface PaymentResultDto {
  id: number;
  paidForDate: string;
  paidAmount: number;
  paymentType: PaymentType,
  payingPerson: string | null;
  licensePlate: string;
}

export interface DetectionDetailDto {
  taken: string;
  multipleCarsOnOneDetection: boolean;
}

export interface PaymentWithDetectionDto {
  paymentId: number;
  paidAmount: number;
  licensePlate: string;
  detectionDetails: DetectionDetailDto[] | null;
}

export interface PaymentAddDto {
  paidAmount: number;
  payingPerson: string | null;
  paymentType: PaymentType;
  licensePlate: string;
}
