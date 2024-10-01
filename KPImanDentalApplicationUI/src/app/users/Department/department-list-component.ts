import { ChangeDetectorRef, Component, OnInit } from "@angular/core";
import { ConfirmationService, MenuItem, MessageService } from "primeng/api";
import Swal from "sweetalert2";
import { DeletionCondition, DepartmentDto, MessageType, PositionDto } from "../../../shared/model/AppModel";
import { userServices } from "../../../shared/_services/user.service";

@Component({
  selector: 'app-department-list',
  templateUrl: 'department-list-component.html',
  styleUrls: ['../user-list-component.css']
})

export class DepartmentListComponent {

  constructor(
    private _service: userServices,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
  ) { }

  departmentList: DepartmentDto[] = [];
  departmentId: number | null = null;
  selectedDepartment!: any;
  departmentCount: number | undefined;

  deletionCondition: DeletionCondition<any> = new DeletionCondition;

  formState: string | undefined;
  modalVisible: boolean = false;

  showErrorDialog: boolean = false;
  errorMessage: string = '';

  actions: MenuItem[] = [];

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
          this.departmentId = this.selectedDepartment.id;
          this.departmentCount = this.departmentList.length;
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

  getAction(department: any) {
    this.actions = [
      {
        label: 'Edit',
        icon: 'pi pi-pencil',
        command: () => {
          this.departmentId = department.id
          this.showEditModal();
        }
      },
      {
        label: 'Delete',
        icon: 'pi pi-trash',
        command: () => {
          this.deleteData(department.id);
        },
      }
    ];

    return this.actions;
  }

  onRowSelect(event: any) {
    this.departmentId = event.data.id;
  }

  deleteData(id: number) {
    this._service.canDeleteDepartment<DepartmentDto>(id).subscribe({
      next: (result: DeletionCondition<DepartmentDto>) => {
        if (result) {
          this.deletionCondition = result;
        }
        //check deletionCondition
        if (this.deletionCondition.canDelete || this.deletionCondition.messageType === MessageType.Warning) {
          this.confirmationService.confirm({
            message: this.deletionCondition.message.replace(/\n/g, '<br>'),
            header: 'Confirm Delete?',
            icon: 'pi pi-info-circle',
            acceptButtonStyleClass: "p-button-danger p-button-text",
            rejectButtonStyleClass: "p-button-text p-button-text",
            acceptIcon: "none",
            rejectIcon: "none",

            accept: () => {
              //do deletion
              this.messageService.add({ severity: 'info', summary: 'Confirmed', detail: 'Record deleted' });
            },
            reject: () => {
              
            }
          });
        }
        else {
          this.showErrorDialog = true;
          this.errorMessage = this.deletionCondition.message;
        }
      }
    });
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

  showModal() {
    this.formState = 'Create';
    this.departmentId = null;
    this.modalVisible = true;
  }

  showEditModal() {
    console.log(this.departmentId)
    this.formState = 'Edit';
    this.modalVisible = true;
  }

  refresh(data: any) {
    this.modalVisible = false;
    this.getData();
    if (data == 'Create') {
      this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Data Saved!!' });
    } else {
      this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Data Updated!!' });
    }
  }
}
