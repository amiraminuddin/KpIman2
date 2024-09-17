import { Injectable } from "@angular/core";

@Injectable()
export class AppConsts {
  //ModalFormMode
  static readonly CreateMode: string = 'CreateMode';
  static readonly EditMode: string = 'EditMode';
  static readonly ViewOnlyMode: string = 'ViewOnlyMode';

  static readonly CreateForm: string = 'Create';
  static readonly EditForm: string = 'Edit';
  static readonly ViewForm: string = 'View';

  ActionMode: any = [
    { code: AppConsts.CreateMode },
    { code: AppConsts.EditMode },
    { code: AppConsts.ViewOnlyMode }
  ];

  constructor() { }

  getActionMode(): any[] {
    return this.ActionMode;
  }

  getActionCreateMode(): string {
    return AppConsts.CreateMode;
  }

  getActionEditMode(): string {
    return AppConsts.EditMode;
  }

  getActionViewOnlyMode(): string {
    return AppConsts.ViewOnlyMode;
  }

  getModeForm(mode: string): string{
    let x: any;
    switch (mode) {
      case AppConsts.CreateMode:
        x = AppConsts.CreateForm
        break;
      case AppConsts.EditMode:
        x = AppConsts.EditForm
        break;
      case AppConsts.ViewOnlyMode:
        x =  AppConsts.ViewForm
        break;
    }
    return x;
  }
}
