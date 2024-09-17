import { Injectable } from "@angular/core";
import { CanActivate, Router } from "@angular/router";
import { Observable } from "rxjs";
import Swal from "sweetalert2";
import { AccountServices } from "../_services/account.service";

@Injectable({
  providedIn: 'root'
})
export class logoutGuard implements CanActivate {

  constructor(
    private accountService: AccountServices,
    private router: Router,
  ) { };

  canActivate(): any {
    if (this.accountService.isLoggedIn()) {
      Swal.fire({
        icon: "warning",
        title: "Opss",
        text: "Please click button logout!!"
      });
      this.router.navigate(['/kpIman']);
      return false;
    }
    else {
      return true;
    }
  };

}
