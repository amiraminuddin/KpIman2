import { Component, ElementRef, Injectable, Output, ViewChild, EventEmitter } from "@angular/core";
import * as bootstrap from 'bootstrap';
import { AppConsts } from "../../../shared/AppConsts";
import { UserRegister } from "../../../shared/model/user";

@Component({
  selector: 'app-user-modal',
  templateUrl: 'user-modal-component.html',
})

export class userModal {

  @ViewChild('userModal') userModal!: ElementRef;

  @Output('callbackRecordCreate') eventEmitRecordCreate = new EventEmitter<any>();

  constructor(
    public _appConsts: AppConsts
  ) { }

  user = new UserRegister;
  modalInstance: bootstrap.Modal | undefined;
  currentMode: string = "";

  ngOnInit(): void {

  }

  ngAfterViewInit() {
    const modalElement = this.userModal.nativeElement;
    this.modalInstance = new bootstrap.Modal(modalElement);
  }

  show(mode: string, userRegister: any) {
    this.currentMode = mode;
    switch (mode) {
      case AppConsts.CreateMode:
        this.user = new UserRegister;
        break;
      case AppConsts.EditMode:
      case AppConsts.ViewOnlyMode:
        this.user = userRegister;
        this.user.birthDate = new Date(userRegister.birthDate);
        break;
    }
    this.modalInstance?.show();
  }

  save() {
    this.eventEmitRecordCreate.emit(this.user);
    this.modalInstance?.hide();
  }

}
