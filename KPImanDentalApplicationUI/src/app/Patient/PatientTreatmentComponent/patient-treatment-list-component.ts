import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { MenuItem } from "primeng/api";
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
    private patientService: PatientService
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
         /* this.deleteData(treatment.id);*/
        },
      }
    ];

    return this.actions;
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

  refresh(event: any) {

  }

}
