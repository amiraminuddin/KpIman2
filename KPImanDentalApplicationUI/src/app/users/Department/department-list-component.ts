import { ChangeDetectorRef, Component, OnInit } from "@angular/core";
import Swal from "sweetalert2";
import { DepartmentDto, PositionDto } from "../../../shared/model/AppModel";
import { userServices } from "../../../shared/_services/user.service";

@Component({
  selector: 'app-department-list',
  templateUrl: 'department-list-component.html',
  styleUrls: ['../user-list-component.css']
})

export class DepartmentListComponent {

  constructor(private _service: userServices, private cdr: ChangeDetectorRef) { }

  departmentList: DepartmentDto[] = [];
  position: PositionDto[] = [];
  selectedDepartment!: any;

  ngOnInit(): void {
    this.getData();
    setTimeout(() => {
      this.setComponentHeight();
    }, 3000); // Delay in milliseconds (3000 ms = 3 seconds)
  }

  getData() {
    this._service.getAllDepartment().subscribe({
      next: response => {
        this.departmentList = response;
        if (this.departmentList) {
          this.selectedDepartment = this.departmentList[0];
          this.position = this.selectedDepartment.position;
        }
      },
      error: error => {
        Swal.fire({
          icon: "error",
          title: "Oops...",
          text: error.error
        });
      }
    })
  }

  onRowSelect(event: any) {
    this.position = event.data.position
  }

  setComponentHeight() {
    var navbarElement = document.getElementById('MainHeader');
    var screenHeight = window.innerHeight; // Get the full screen height
    var element = document.getElementById('departmentListId');
    //var static = 38;

    if (navbarElement) {
      const navbarHeight = navbarElement.offsetHeight; // Safe to access
      const componentHeigth = screenHeight - navbarHeight - 38;
      if (element) {
        element.style.height = `${componentHeigth}px`; // Set height in pixels
      }
    } else {
      console.error('Navbar element not found');
    }
  }

}
