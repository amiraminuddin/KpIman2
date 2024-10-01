import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { ConfirmationService, MenuItem, MessageService } from "primeng/api";
import { PatientDto, PatientTreatmentDtoExt } from "../../../shared/model/AppModel";
import { PatientService } from "../../../shared/_services/patient.service";

@Component({
  selector: 'patient-treatment-list',
  templateUrl : 'patient-treatment-list-component.html'
})

export class PatientTreatmentList implements OnInit {

  constructor(
    private service: PatientService,
    private activeRoute: ActivatedRoute,
    private patientService: PatientService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
  ) { }

  patientId: number = -1
  patientTreatments: PatientTreatmentDtoExt[] = [];
  patientInfo = new PatientDto;
  treatmentCount: number = 0
  actions: MenuItem[] = [];

  modalState: string | undefined;
  modalVisible: boolean = false;
  treatmentId: number | undefined;


  ngOnInit(): void {
    this.patientId = Number(this.activeRoute.snapshot.paramMap.get('patientId'));
    this.getPatientInfo();
    this.getData();
  }


  getData() {
    this.service.getAllPatientTreatment(this.patientId).subscribe(x => {
      this.patientTreatments = x;
      if (x.length > 0) {
        this.treatmentCount = x.length;
      }
    });
  }

  getPatientInfo() {
    this.patientService.getPatientsById(this.patientId).subscribe(x => {
      this.patientInfo = x;
    })
  }

  getAction(treatment: any) {
    this.actions = [
      {
        label: 'Edit',
        icon: 'pi pi-pencil',
        command: () => {
          this.showTreatmentModal(treatment.id)
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


  deleteData(id: any) {
    console.log('deleteService');
    this.confirmationService.confirm({
      message: 'Are sure to delete?',
      header: 'Confirm Delete?',
      icon: 'pi pi-info-circle',
      acceptButtonStyleClass: "p-button-danger p-button-text",
      rejectButtonStyleClass: "p-button-text p-button-text",
      acceptIcon: "none",
      rejectIcon: "none",
      accept: () => {
        //do deletion
        this.service.deletePatientTreatment(id).subscribe({
          next: _ => {
            this.getData();
            this.messageService.add({ severity: 'info', summary: 'Success', detail: 'Data Deleted!!' });
          }
        })
      },
      reject: () => {

      }
    })
  }

  showTreatmentModal(id?: number) {
    if (id == null) {
      this.modalState = 'Create';
      this.treatmentId = undefined;
      this.modalVisible = true;
    } else {
      this.modalState = 'Edit';
      this.treatmentId = id;
      this.modalVisible = true;
    }
  }

  refresh(data: any) {
    this.modalVisible = false;
    this.getData();
    if (data == 'Create') {
      this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Data Saved!!' });
    } else {
      this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Data Updated!!' });
    }
    //this.getData();
  }

}
