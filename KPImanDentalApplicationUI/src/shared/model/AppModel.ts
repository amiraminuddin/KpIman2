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
  fullName: string | undefined;
  email: string | undefined;
  position: string | undefined;
  department: string | undefined;
  birthDate: string | undefined;
  address: string | undefined;
  isActive: boolean | undefined;
  isSupervisor: boolean | undefined;
  supervisorId: number | undefined;
  gender: string | undefined;
  userPhoto: string | undefined;
  hierarchyLevel: number | undefined;
}

export class UserDtoExt extends UserDto {
  rowNumber: number | undefined;
  departmentL: LookupTemplateDto | undefined;
  positionL: LookupTemplateDto | undefined;
  supervisorNameL: LookupTemplateDto | undefined;
  convertDateTime: string | undefined;

}

export class UserCreateDto extends UserDtoExt {
  userName: string | undefined;;
  Password: string | undefined;;
  ConfirmPassword: string | undefined;;
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

export class PatientTreatmentDto {
  id: number | undefined;
  patientId: number | undefined;
  drID: number | undefined;
  dSAId: number | undefined;
  treatmentNo: string | undefined;
  condition: string | undefined;
  description: string | undefined;
  treatmentType: number | undefined;
  treatmentCost: number | undefined;
  treatmentDate: string | undefined;
  prescribedMedical: string | undefined;
  treatmentNotes: string | undefined;
  followUpReq: boolean | undefined;
  followUpDate: string | undefined;
}

export class PatientTreatmentDtoExt extends PatientTreatmentDto {
  patientName: string | undefined;
  doctor: LookupTemplateDto | undefined;
  dsa: LookupTemplateDto | undefined;
  treatment: LookupTemplateDto | undefined;
  treatmentDateDisplay: string | undefined
}

//END : Patient Model

export class LookupTemplateDto {
  fieldValue: string | undefined;
  fieldDisplay: string | undefined;
}

export class TreatmeantLookupDto {

  id: number | undefined; 
  treatmentCode: string | undefined;
  treatmentName: string | undefined;
  treatmentDesc: string | undefined;
  isActive?: boolean;
  treatmentPrice: number | undefined;
}

export class PatientLookupDto {
  id: number | undefined;
  firstName: string | undefined;
  lastName: string | undefined;
  email: string | undefined;
  contactNo: string | undefined;
}

export class StaffLookupDto {
  id: number | undefined;
  userName: string | undefined;
  email: string | undefined;
  position: string | undefined;
  department: string | undefined;
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
export enum ValidatorsType {
  Error,
  Warning,
  Mandatory
}

export enum ValidatorTriggerType {
  OnLoad,
  OnChange,
  OnSave  
}

export interface Column {
  field: string;
  header: string;
  type: string;
  sortable: boolean;
}

export interface SelectedLookup {
  FieldValue: any;
  FieldDisplay: string;
}

export class Validators {
  field: string | undefined;
  isValid: boolean | undefined;
  message: string | undefined;
  validatorsType: ValidatorsType | undefined;
}

export class DataValidator<T> {
  data: T | undefined;
  triggerType: ValidatorTriggerType | undefined;
}

export class ActionValidatorsInput<T>{
  data: T | undefined;
  actionCode: string[] | undefined;
}

export class ActionValidatorsOutput {
  actionCode: string | undefined;
  isDisabled: boolean | undefined;
  isLocked: boolean | undefined;
  isVisible: boolean | undefined;
  lockedMessage: string | undefined;
}

export class OrganizationChartDto {
  label: string | undefined;
  type: string | undefined;
  styleClass: string | undefined;
  expanded: boolean | undefined;
  data: OrganizationChartDataDto | undefined;
  children: OrganizationChartDto[] | undefined;
}

export class OrganizationChartDataDto {
  name: string | undefined;
  profilePicture: string | undefined;
}

export class gridDto<T>{
  totalData: number | undefined;
  data: T | undefined;
  pageSize: number | undefined;
  totalPages: number | undefined;
}

export class GridInputDto {
  currentPage: number | undefined;
  pageSize: number | undefined;
  sortMeta: SortMeta[] | undefined;
  filterValue: string | undefined;
  filterColumn: FilterColumn[] | undefined;
  whereCondition: WhereClause[] | undefined;
}

export class SortMeta {
  field: string | undefined;
  order: number | undefined
}

export class FilterColumn {
  field: string | undefined;
}

export class WhereClause {
  field: string | undefined;
  value: string | undefined;
}
