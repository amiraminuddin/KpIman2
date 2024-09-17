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
