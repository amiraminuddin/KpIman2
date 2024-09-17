import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";
import Swal from "sweetalert2";
import { AccountServices } from "../_services/account.service";


@Injectable({
  providedIn: 'root' // This registers the guard as a provider
})

export class authGuard implements CanActivate {

  constructor(
    private accountService: AccountServices,
    private router: Router
  ) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): any {
    if (this.accountService.isLoggedIn()) {
      return true;
    } else {
      Swal.fire({
        icon: "error",
        title: "Oops...",
        text: "Please login!!"
      });
      this.router.navigate(['/login']);
      return false;
    }      
  }
};
