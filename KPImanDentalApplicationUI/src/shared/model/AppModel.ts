import { Byte } from "@angular/compiler/src/util";
import { Injectable } from "@angular/core";
import { Message } from "primeng/api";

@Injectable()
export class UserConst {
  constructor() {

  }
}
export class UserRegister {
  userName: string = '';
  password: string = '';
  email: string = '';
  position: string = '';
  department: string = '';
  birthDate: Date = new Date;
  address: string = '';
  isActive: boolean = true;
  isSupervisor: boolean = false;
}

export class UserDto {
  id: number | undefined;
  userName: string = '';
  email: string = '';
  position: string = '';
  department: string = '';
  birthDate: Date = new Date;
  address: string = '';
  isActive: boolean = true;
  isSupervisor: boolean = false;
  gender: string = '';
  formattedBirthDate: string = '';
  userPhoto: string = '';
}

export class ModuleUpdateDto {
  moduleDescription: string = '';
  isActive: boolean = true;
  moduleIcon: string = '';
}

export class DepartmentDto {
  id: number | undefined;
  code: string = '';
  name: string = '';
  description: string = '';
}

export class PositionDto {
  id: number | undefined;
  departmentId: number | undefined;
  departmentName: string = '';
  code: string = '';
  name: string = '';
  description: string = '';
}



//START : Patient Model
export class PatientDto {
  id: number | undefined;
  firstName : string = '';
  lastName : string = '';
  email : string  = '';
  dateOfBirth: Date = new Date;
  gender : string  = '';
  contactNo : string  = '';
  address: string  = '';
  isActive: boolean = true;
}

export class PatientTreamentFormDto {
  id: number | undefined;
  patient: any | undefined;
  dr: any | undefined; // Doctor in charge
  dsa: any | undefined; // DSA in charge
  treatmentNo: string | undefined;
  condition: string | undefined;
  description: string | undefined;
  treatmentType: number | undefined; // Treatment Lookup
  treatmentCost: number | undefined; // Get from Treatment Lookup by default
  treatmentDate: Date | undefined;
  prescribedMedical: string | undefined;
  treatmentNotes: string | undefined;
  followUpReq?: boolean; // Nullable
  followUpDate?: Date; // Nullable
  createdBy: string = "System"; // Default
  createdOn: Date | undefined;
  updatedBy: string | undefined;
  updatedOn: Date | undefined;
}

//END : Patient Model

export class TreatmeantLookupDto {

  id: number | undefined; 
  treatmentCode: string | undefined;
  treatmentName: string | undefined;
  treatmentDesc: string | undefined;
  isActive?: boolean;
  treatmentPrice: number | undefined;
  createdBy: string | undefined;
  updatedBy: string | undefined;
}


export class DeletionCondition<T> {
  entity: T | undefined;
  dependenciesCount: number | undefined;
  hasDependencies: boolean | undefined;
  message: string = "";
  messageType: MessageType | undefined;
  canDelete: boolean | undefined;
}

export enum MessageType {
  Error,
  Warning,
  Information
}
