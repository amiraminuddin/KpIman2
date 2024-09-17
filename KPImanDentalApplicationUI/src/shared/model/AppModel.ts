import { Byte } from "@angular/compiler/src/util";
import { Injectable } from "@angular/core";

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
  position: PositionDto = new PositionDto;
}

export class PositionDto {
  id: number | undefined;
  departmentId: number | undefined;
  departmentCode: string = '';
  code: string = '';
  name: string = '';
  description: string = '';
}
