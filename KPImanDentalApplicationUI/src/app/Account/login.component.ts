import { Component, Injectable } from "@angular/core";
import { FormsModule } from '@angular/forms';
import { Router } from "@angular/router";
import { AccountServices } from "../../shared/_services/account.service";
import Swal from 'sweetalert2';
import { timeout } from "rxjs";

@Component({
  selector: "app-login-component",
  templateUrl: "login.component.html",
  styleUrls: ["login.component.css"]
})

export class LoginComponent {
  user: any = {};

  isLoginSuccesful: boolean = false;
  result: any;

  isLoad: boolean = false;

  constructor(
    private service: AccountServices,
    private router: Router
  ) { }

  login() {
    this.isLoad = true;
    setTimeout(() => {
      this.service.login(this.user).subscribe(
        (response: any) => {
          this.service.storeToken(response);
          this.router.navigate(['kpIman/main']);
        },
        (error) => {
          Swal.fire({
            icon: "error",
            title: "Oops...",
            text: error.error
          });
          this.isLoad = false;
        }
      )   
    }, 3000)
       
  }
}
