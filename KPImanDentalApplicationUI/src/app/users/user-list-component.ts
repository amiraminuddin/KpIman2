import { HttpClient } from "@angular/common/http";
import { Component, Injectable, ViewChild, AfterViewInit, OnInit } from "@angular/core";
import { Observable, timeout } from "rxjs";
import Swal from "sweetalert2";
import { environment } from "../../environments/environment";
import { AppConsts } from "../../shared/AppConsts";
import { AccountServices } from "../../shared/_services/account.service";
import { ModuleService } from "../../shared/_services/module.service";
import { userModal } from "./user-modal/user-modal-component";

@Component({
  selector: 'app-user-component',
  templateUrl: 'user-list-component.html',
  styleUrls: ['user-list-component.css']
})

export class userComponent implements OnInit {

  private apiUrl = environment.apiUrl //todo take from services
  users: any;
  isLoad: boolean = false;

  formState: string | undefined;
  modalVisible: boolean = false;

  constructor(
    private http: HttpClient,
    private service: AccountServices,
    private module: ModuleService,
  )
  { }

  ngOnInit(): void {
    this.module.GetModuleStatus(7).then(isActive => {
      if (isActive) {
        this.getData();
      }
    });
    setTimeout(() => {
      this.setComponentHeight();
    }, 3000); // Delay in milliseconds (3000 ms = 3 seconds)
  }

  ngAfterViewInit() {
  }

  getData(): Observable<any> {
    this.isLoad = true;
    this.http.get<any>(this.apiUrl + "Users/getAllUser").subscribe({
      next: response => this.users = response,
      error: error => console.log(error),
      complete: () => this.isLoad = false
    });
    return this.users
  }

  getAction(user: any) {


  }

  handleRegisterNewUser(data: any) {
    data.createdBy = this.service.getCurrentUserLogin();
    this.service.register(data).subscribe({
      next: response => {
        this.getData();
      },
      error: error => {
        console.log(error.error.errors)
      }
    })
  }

  deleteUser(id: any) {
    Swal.fire({
      title: "Do you want to delete the record",
      showDenyButton: true,
      confirmButtonText: "Delete",
      denyButtonText: `Cancel`
    }).then((result) => {
      /* Read more about isConfirmed, isDenied below */
      if (result.isConfirmed) {
        this.service.deleteUser(id).subscribe({
          next: response => {
            this.getData();
          },
          error: error => console.log(error)
        });
      }
    });


  }

  EditUser(user: any) {
  }

  showModal() {
    this.formState = "Create";
    this.modalVisible = true;
  }

  refresh(event: any) {

  }

  setComponentHeight() {
    var navbarElement = document.getElementById('MainHeader');
    var screenHeight = window.innerHeight; // Get the full screen height
    var element = document.getElementById('userListId');
    //var static = 38;

    if (navbarElement) {
      const navbarHeight = navbarElement.offsetHeight; // Safe to access
      const componentHeigth = screenHeight - navbarHeight - 38;
      if (element) {
        element.style.height = `${componentHeigth}px`; // Set height in pixels
      }
    }
  }
}
