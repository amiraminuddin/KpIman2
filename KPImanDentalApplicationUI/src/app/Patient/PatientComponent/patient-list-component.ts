import { Component, OnInit } from "@angular/core";
import { MenuModule } from 'primeng/menu';
import { MenuItem, MessageService } from 'primeng/api';
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
  patient: any;
  patientId: number | null = null;
  modalVisible: boolean = false;
  formState: string | undefined;

  constructor(
    private patientService: PatientService,
    private router: Router,
    private messageService: MessageService) {
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
          //this.router.navigate(['/kpIman/Patient/TreatmentForm']);
          window.open(this.router.serializeUrl(this.router.createUrlTree(['/kpIman/Patient/TreatmentForm'])), '_blank');
        }
      },
      {
        label: 'View Medical Treatment',
        icon: 'pi pi-folder-open',
        command: () => {
          window.open(this.router.serializeUrl(this.router.createUrlTree([`/kpIman/Patient/TreatmentList/${patient.id}`])), '_blank');
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
    this.formState = 'Edit';
    this.patientId = id;
    this.modalVisible = true;
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

  addPatient() {
    this.formState = 'Create';
    this.patientId = null;
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
