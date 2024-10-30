import { Component, OnInit } from "@angular/core";
import { MenuItem, MessageService } from "primeng/api";
import Swal from "sweetalert2";
import { Column, FilterColumn, GridInputDto, SortMeta, TreatmeantLookupDto, WhereClause } from "../../../shared/model/AppModel";
import { LookupService } from "../../../shared/_services/lookup.service";
import { GridServiceService } from "../../shared/services/grid/grid-service.service";

@Component({
  selector: 'treatment-list-component',
  templateUrl: 'treatmentL.list.component.html',
  styleUrls: ['../../Patient/PatientComponent/patient-list-component.css']
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
  sortMetaInput: SortMeta[] = [];
  filterCol: FilterColumn[] = [];
  filterLegend: string = "";
  whereClause: WhereClause[] = [];

  private userTriggeredSort: boolean = false;

  constructor(
    private service: LookupService,
    private messageService: MessageService,
    private gridService: GridServiceService,
  ) { }


  ngOnInit(): void {
    this.gridInput.currentPage = 1;
    this.gridInput.pageSize = 5;
    this.getData();      
  }

  getData() {
    let whereC = this.getWhereClause();
    this.gridInput.whereCondition = whereC;
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
          this.filterLegend = "Code, Name, Price";
        },
        complete: () => {
          this.isLoad = false;
        },
        error: () => {
          this.isLoad = false;
          Swal.fire({
            title: "Error",
            text: "Internal Error",
            icon: "error"
          });
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

    let gridSort = this.gridService.handleSortData(event, this.gridInput)

    if (gridSort.hasChanges) {
      this.gridInput.sortMeta = gridSort.newSortMeta;
      this.getData(); // Call API only if sortMeta has changed
    }    
  }

  handleGridFilter(event: any) {
    this.filterCol = [
      { field: "TreatmentCode" },
      { field: "TreatmentName" },
      { field: "TreatmentPrice" }
    ];

    this.gridInput.filterColumn = this.filterCol;
    this.gridInput.filterValue = event
    this.getData();
    //console.log(event);
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

  private getWhereClause() {
    this.whereClause = [];
    //this.whereClause.push({
    //  field: "isActive",
    //  value: "true"
    //});
    return this.whereClause;
  }
}
