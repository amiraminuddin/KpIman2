import { Component, OnInit } from "@angular/core";
import { MenuItem, MessageService } from "primeng/api";
import Swal from "sweetalert2";
import { Column, TreatmeantLookupDto } from "../../../shared/model/AppModel";
import { LookupService } from "../../../shared/_services/lookup.service";

@Component({
  selector: 'treatment-list-component',
  templateUrl: 'treatmentL.list.component.html',
  styleUrls: ['../../Patient/PatientComponent/patient-list-component.css']
  //current location \Lookup\Treatment\treatmentL.list.component.html

})

export class TreatmentListComponent implements OnInit {

  treatmeantList: TreatmeantLookupDto[] = [];
  actions: MenuItem[] = [];
  modalVisible: boolean = false;
  treatmentId: number | null = null;
  formState: string | undefined;


  //for grid
  isLoad: boolean = true;
  gridData: any[] = [];
  gridColumn!: Column[];
  gridDataKey: string = "";
  gridAction: any;
  constructor(private service: LookupService, private messageService: MessageService) { }


  ngOnInit(): void {
    setTimeout(() => {
      this.isLoad = true;
      this.getData();
    },2000);      
  }

  getData() {
    this.service.getAllTreatment().subscribe({
      next: (result: any[]) => {
        if (result) {
          this.gridData = result;
        }

        this.gridColumn = [
          { field: 'action', header: 'Action', type: 'action' },
          { field: 'treatmentCode', header: 'Code', type: 'string' },
          { field: 'treatmentName', header: 'Name', type: 'string' },
          { field: 'treatmentDesc', header: 'Description', type: 'string' },
          { field: 'isActive', header: 'Active?', type: 'bool' },
          { field: 'treatmentPrice', header: 'Price (RM)', type: 'currency' },
        ];

        this.gridDataKey = "treatmentCode"
        this.gridAction = this.getAction();
      },
      complete: () => {
        this.isLoad = false;
      }
    });   
  }

  handleAction(event: any) {
    console.log(event);
    if (event.action.label == "Edit") {
      this.editData(event.rowData.id);
    }
    if (event.action.label == "Delete") {
      this.deleteData(event.rowData.id);
    }
  }

  getAction() {
    return [
      {
        label: 'Edit',
        icon: 'pi pi-pencil'
      },
      {
        label: 'Delete',
        icon: 'pi pi-trash'
      }
    ];
  }

  editData(id: any) {
    this.showEditModal(id);
  }

  deleteData(id: any) {
    Swal.fire({
      title: "Do you want to delete the record",
      showDenyButton: true,
      confirmButtonText: "Delete",
      denyButtonText: `Cancel`
    }).then((result) => {
      /* Read more about isConfirmed, isDenied below */
      if (result.isConfirmed) {
        this.service.deleteTreatment(id).subscribe({
          next: _ => {
            this.getData();
            this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Data Deleted!!' });
          },
          error: _ => { },
        });
      }
    });
  }

  showModal() {
    this.formState = 'Create';
    this.treatmentId = null;
    this.modalVisible = true;
  }

  showEditModal(Id: number) {
    this.formState = 'Edit';
    this.treatmentId = Id
    this.modalVisible = true;
  }

  refresh(data: any) {
    this.modalVisible = false;
    this.isLoad = true;
    setTimeout(() => {
      this.getData();
    }, 2000);
    if (data == 'Create') {
      this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Data Saved!!' });
    } else {
      this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Data Updated!!' });
    }
  }

  selectedGrid(event: any) {
    console.log(event);
  }

}
