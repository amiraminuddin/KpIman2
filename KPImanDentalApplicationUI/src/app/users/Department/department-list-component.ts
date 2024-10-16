import { Component, ViewChild } from "@angular/core";
import { ConfirmationService, MenuItem, MessageService } from "primeng/api";
import { Menu } from "primeng/menu";
import Swal from "sweetalert2";
import { ActionValidatorsInput, ActionValidatorsOutput, DepartmentDto } from "../../../shared/model/AppModel";
import { userServices } from "../../../shared/_services/user.service";

@Component({
  selector: 'app-department-list',
  templateUrl: 'department-list-component.html',
  styleUrls: ['../user-list-component.css']
})

export class DepartmentListComponent {

  @ViewChild('menu') menu!: Menu;
  constructor(
    private _service: userServices,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
  ) { }

  departmentList: DepartmentDto[] = [];
  departmentId: number | null = null;
  selectedDepartment!: any;
  departmentCount: number | undefined;

  actionValidatorInput: ActionValidatorsInput<DepartmentDto> = new ActionValidatorsInput;

  formState: string | undefined;
  modalVisible: boolean = false;

  showErrorDialog: boolean = false;
  errorMessage: string = '';

  actions: MenuItem[] = [];

  activeDepartment: any = null;

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

  onMenuButtonClick(department: any, event: MouseEvent) {
    this.getAction(department);
    this.menu.toggle(event);
  }

  getAction(department: any) {
    this.actionValidatorInput.data = department;
    this.actionValidatorInput.actionCode = ['EDIT', 'DELETE'];

    this._service.getDepartmentActionValidator(this.actionValidatorInput).subscribe({
      next: (result: ActionValidatorsOutput[]) => {
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
            disabled: result?.find(x => x.actionCode == 'DELETE')?.isDisabled,
            visible: result?.find(x => x.actionCode == 'DELETE')?.isVisible,
            command: () => {
              let formula = result?.find(x => x.actionCode == 'DELETE');
              this.deleteData(department.id, formula);
            },
          }
        ];
      }
    });

    return this.actions;
  }

  toggleDropdown(department: any, event: MouseEvent) {
    if (this.activeDepartment === department) {
      this.activeDepartment = null;
    } else {
      this.activeDepartment = department;
    }
    event.stopPropagation();
  }

  onRowSelect(event: any) {
    this.departmentId = event.data.id;
  }

  deleteData(department: any, formula: any) {
    if (formula) {
      this.errorMessage = formula.lockedMessage;
      this.showErrorDialog = true;
    }
    else {
      //todo: delete data
      this.confirmationService.confirm({
        message: 'Do you want to delete this record?',
        header: 'Delete Confirmation',
        icon: 'pi pi-info-circle',
        accept: () => {
          this._service.deleteDepartment(department).subscribe({
            next: _ => {
              this.getData();
              this.messageService.add({ severity: 'info', summary: 'Confirmed', detail: 'Record deleted' });
            }
          })
        },
        reject: () => {
          //this.messageService.add({ severity: 'info', summary: 'Confirmed', detail: 'okay' });
        }
      });
    }
    
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
