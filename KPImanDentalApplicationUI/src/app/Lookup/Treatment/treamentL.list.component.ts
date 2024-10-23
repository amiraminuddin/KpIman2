import { Component, OnInit } from "@angular/core";
import { MenuItem, MessageService } from "primeng/api";
import Swal from "sweetalert2";
import { Column, GridInputDto, TreatmeantLookupDto } from "../../../shared/model/AppModel";
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

  dataCount: number = 0;
  //for grid
  isLoad: boolean = true;
  gridData: any[] = [];
  gridColumn!: Column[];
  gridDataKey: string = "";
  gridAction: any;
  gridInput: GridInputDto = new GridInputDto;

  constructor(private service: LookupService, private messageService: MessageService) { }


  ngOnInit(): void {
    this.gridInput.currentPage = 1;
    this.gridInput.pageSize = 5;
    this.getData();      
  }

  getData() {
    this.isLoad = true;
    setTimeout(() => {
      this.service.getGridTreatment(this.gridInput).subscribe({
        next: response => {
          if (response) {
            this.gridData = response.data || [];
          }
          this.gridColumn = [
            { field: 'action', header: 'Action', type: 'action', sortable: false },
            { field: 'treatmentCode', header: 'Code', type: 'string', sortable: true },
            { field: 'treatmentName', header: 'Name', type: 'string', sortable: true },
            { field: 'treatmentDesc', header: 'Description', type: 'string', sortable: false },
            { field: 'isActive', header: 'Active ?', type: 'bool', sortable: false },
            { field: 'treatmentPrice', header: 'Price (RM)', type: 'currency', sortable: true },
          ];

          this.gridDataKey = "treatmentCode"
          this.dataCount = response.totalData || 0;
          this.gridAction = this.getAction();
        },
        complete: () => {
          this.isLoad = false;
        }
      });
    }, 1000);    
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

  handlePageChange(event: any) {
    this.gridInput.currentPage = event.page + 1;
    this.gridInput.pageSize = event.rows;
    this.getData();
  }

  handleSortData(event: any) {
    if (this.gridInput.sortableInput !== event.field || this.gridInput.sortableMode !== event.orderByMode) {
      this.gridInput.sortableMode = event.orderByMode;
      this.gridInput.sortableInput = event.field;
      this.getData();
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
    this.getData();
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
