import { Component, OnInit } from "@angular/core";
import { MenuModule } from 'primeng/menu';
import { MenuItem } from 'primeng/api';
import { PatientDto } from "../../../shared/model/AppModel";
import { PatientService } from "../../../shared/_services/patient.service";
import { Router } from "@angular/router";

@Component({
  selector: 'patient-list-component',
  templateUrl: 'patient-list-component.html',
  styleUrls: ['patient-list-component.css']
})
export class PatientListComponent implements OnInit {

  patients : PatientDto[] = [];
  actions: MenuItem[] = [] ;
  patient: any

  constructor(private patientService: PatientService, private router: Router) {
  }

  ngOnInit(): void {
    this.getData();
  }

  getData() {
    this.patientService.getAllPatients().subscribe(x => {
      this.patients = x;
    })
  }

  getAction(patient: any) {
    this.actions = [
      {
        label: 'Edit',
        icon: 'pi pi-pencil',
        command: () => {
          this.editData(patient.id);
        }
      },
      {
        label: 'Register Medical Treatment',
        icon: 'pi pi-plus-circle',
        command: () => {
          this.router.navigate(['/kpIman/Patient/TreatmentForm/', patient.id]);
        }
      },
      {
        label: 'View Medical Treatment',
        icon: 'pi pi-folder-open',
        command: () => {
          this.router.navigate(['/kpIman/Patient/TreatmentForm'])
        }
      },
      {
        label: 'Delete',
        icon: 'pi pi-trash',
        command: () => {
          this.editData(patient.id);
        },
      }
    ];

    return this.actions;
  }

  editData(id: any) {
    console.log(id);
  }

  deleteData(id: any) {
    console.log(id);
  }

  addMedicalTreatment(id: any) {
    //TODO: Open treatment Form
  }

  viewMedicalTreatment(id: any) {
    //TODO: View treatment Form
  }

}
