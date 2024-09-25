import { ChangeDetectorRef, Component, Input, OnChanges, SimpleChanges } from "@angular/core";
import { MenuItem, MessageService } from "primeng/api";
import Swal from "sweetalert2";
import { PositionDto } from "../../../shared/model/AppModel";
import { userServices } from "../../../shared/_services/user.service";

@Component({
  selector: 'app-position-list-component',
  templateUrl: 'position-list-component.html',
  styleUrls: ['../user-list-component.css']
})
export class positionComponent implements OnChanges {
  @Input() departmentId: number | null | undefined;

  positionData: PositionDto[] = [];
  positionId: number | null = null;
  positionCount: number | undefined;

  formState: string | undefined;
  modalVisible: boolean = false;

  actions: MenuItem[] = [];

  constructor(private cdr: ChangeDetectorRef, private service: userServices, private messageService: MessageService) { }

  ngOnInit() {
    this.getData();
    this.cdr.detectChanges();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes["departmentId"] && this.departmentId) {
      this.getData();
    }
  }

  getData() {
    if (this.departmentId != undefined) {
      this.service.getPositionByDeprtmId(this.departmentId).subscribe({
        next: response => {
          this.positionData = response;
          if (this.positionData.length == 0) {
            this.positionCount = 0;
          }
          else {
            this.positionCount = this.positionData.length;
          }
        },
        error: error => { console.log(error) }
      });
    }
  }

  getAction(position: any) {
    this.actions = [
      {
        label: 'Edit',
        icon: 'pi pi-pencil',
        command: () => {
          this.positionId = position.id
          this.showEditModal();
        }
      },
      {
        label: 'Delete',
        icon: 'pi pi-trash',
        command: () => {
          this.deleteData(position.id);
        },
      }
    ];

    return this.actions;

  }

  showModal() {
    this.formState = 'Create';
    this.positionId = null;
    this.modalVisible = true;
  }

  showEditModal() {
    this.formState = 'Edit';
    this.modalVisible = true;
  }

  deleteData(id: any) {
    this.service.canDeletePosition(id).subscribe({
      next: (result) => {
        if (result.canDelete) {
          Swal.fire({
            title: "Do you want to delete the record",
            showDenyButton: true,
            confirmButtonText: "Delete",
            denyButtonText: `Cancel`
          }).then((result) => {
            if (result.isConfirmed) {
              this.service.deletePosition(id).subscribe({
                next: _ => {
                  this.getData();
                  this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Data Deleted!!' });
                },
                error: _ => { },
              });
            }
          });
        }
        else {
          Swal.fire(result.message);
        }
      }
    })

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
