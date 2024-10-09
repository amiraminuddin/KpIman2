import { HttpClient } from "@angular/common/http";
import { Component, OnInit, ViewChild } from "@angular/core";
import { MenuItem } from "primeng/api";
import { Menu } from "primeng/menu";
import { Observable, timeout } from "rxjs";
import Swal from "sweetalert2";
import { environment } from "../../environments/environment";
import { AppConsts } from "../../shared/AppConsts";
import { UserDtoExt } from "../../shared/model/AppModel";
import { AccountServices } from "../../shared/_services/account.service";
import { ModuleService } from "../../shared/_services/module.service";
import { userServices } from "../../shared/_services/user.service";
import { userModal } from "./user-modal/user-modal-component";

@Component({
  selector: 'app-user-component',
  templateUrl: 'user-list-component.html',
  styleUrls: ['user-list-component.css']
})

export class userComponent implements OnInit {

  @ViewChild('menu') menu!: Menu;

  users: UserDtoExt[] = [];
  isLoad: boolean = false;

  formState: string | undefined;
  modalVisible: boolean = false;
  actions: MenuItem[] = [];
  userId: number | null | undefined;

  constructor(
    private http: HttpClient,
    private service: AccountServices,
    private module: ModuleService,
    private userService: userServices,
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

  getData() {
    this.isLoad = true;
    //this.http.get<any>(this.apiUrl + "Users/getAllUser").subscribe({
    //  next: response => this.users = response,
    //  error: error => console.log(error),
    //  complete: () => this.isLoad = false
    //});
    this.userService.getAllUser().subscribe({
      next: response => {
        this.users = response;
        this.isLoad = false
      }
    })
    /*return this.users*/
  }

  onMenuButtonClick(user: any, event: MouseEvent) {
    this.getAction(user);
    this.menu.toggle(event);
  }

  getAction(user: any) {

    this.actions = [
      {
        label: 'Edit',
        icon: 'pi pi-pencil',
        command: () => {
          this.EditUser(user);
        }
      },
      {
        label: 'Delete',
        icon: 'pi pi-trash',
        command: () => {

        }
      }
    ];

    return this.actions;
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
    this.formState = "Edit";
    this.modalVisible = true;
    this.userId = user.id;
  }

  showModal() {
    this.userId = null;
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
