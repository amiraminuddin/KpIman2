import { Component, OnInit } from "@angular/core";
import { MenuItem, MessageService } from "primeng/api";
import Swal from "sweetalert2";
import { TreatmeantLookupDto } from "../../../shared/model/AppModel";
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

  constructor(private service: LookupService, private messageService: MessageService) { }


  ngOnInit(): void {
    this.getData();
  }

  getData() {
    this.service.getAllTreatment().subscribe(x => {
      this.treatmeantList = x;
    });
  }

  getAction(treatment: any) {
    this.actions = [
      {
        label: 'Edit',
        icon: 'pi pi-pencil',
        command: () => {
          this.editData(treatment.id);
        }
      },
      {
        label: 'Delete',
        icon: 'pi pi-trash',
        command: () => {
          this.deleteData(treatment.id);
        },
      }
    ];

    return this.actions;
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

}
