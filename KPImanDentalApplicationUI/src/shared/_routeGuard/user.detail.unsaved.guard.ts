import { Component, Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot, UrlTree } from "@angular/router";
import { map, Observable, switchMap } from "rxjs";
import Swal from "sweetalert2";
import { UserDetailComponent } from "../../app/users/user-detail-component";
import { AccountServices } from "../_services/account.service";
import { userServices } from "../_services/user.service";


@Injectable({
  providedIn: 'root'
})
export class UserDetailUnsavedGuard implements CanDeactivate<UserDetailComponent>  {

  constructor(private service: userServices, private accService: AccountServices) { }

  hasUserChange: any;

  canDeactivate(component: UserDetailComponent): Promise<boolean> {
    //TODO: Handle when loggout
    if (this.accService.isLogOut) {
      return Promise.resolve(true); // Allow navigation if logging out
    }

    return this.service.checkUserChange(component.user).toPromise().then(response => {
      if (response) {
        return true;
      } else {
        return this.showUnsavedChangesPrompt();
      }
    });
  }


  private async showUnsavedChangesPrompt(): Promise<boolean> {
    const result = await Swal.fire({
      title: 'You have unsaved changes. Do you want to continue?',
      showDenyButton: true,
      confirmButtonText: 'Yes',
      denyButtonText: 'Cancel',
    });

    return result.isConfirmed; // Return true if "Yes" is clicked, false otherwise
  }

}
