import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";
import { AppConsts } from "../../../shared/AppConsts";
import { PatientTreamentFormDto } from "../../../shared/model/AppModel";
import { PatientService } from "../../../shared/_services/patient.service";

@Component({
  selector: 'patient-treatment-form-component',
  templateUrl: 'patient-treatment-form-component.html',
  styleUrls: ['patient-treatment-form-component.css']
})

//will use Reactive Form
export class PatientTreatmentFormComponent implements OnInit {

  constructor(
    private service: PatientService,
    private appConsts: AppConsts,
    private activeRoute: ActivatedRoute) {
    this.genderList = appConsts.getGenderList();
  }

  patientTreatment: PatientTreamentFormDto = new PatientTreamentFormDto;
  patientTreatmentForm: FormGroup = new FormGroup({});
  genderList: any[] = []

  ngOnInit(): void {
    var userId = this.activeRoute.snapshot.paramMap.get('id');
    if (userId) {
      this.initializeViewForm();
      this.getData();      
    } else {
      this.initializeCreateForm();
    }    
  }

  initializeViewForm() {
    this.patientTreatmentForm = new FormGroup({
      patient: new FormGroup({
        id: new FormControl(),
        firstName: new FormControl({ value: '', disabled: true }),
        email: new FormControl({ value: '', disabled: true }),
        dateOfBirth: new FormControl({ value: '', disabled: true }),
        gender: new FormControl({ value: '', disabled: true }),
        contactNo: new FormControl({ value: '', disabled: true }),
        address: new FormControl({ value: '', disabled: true })
      }),
      dr: new FormGroup({
        id: new FormControl(),
        userName: new FormControl({ value: '', disabled: true }),
        email: new FormControl({ value: '', disabled: true })
      }),
      dsa: new FormGroup({
        id: new FormControl(),
        userName: new FormControl({ value: '', disabled: true }),
        email: new FormControl({ value: '', disabled: true })
      }),
      treatmentNo: new FormControl({ value: '', disabled: true }),
      condition: new FormControl(),
      description: new FormControl(),
      treatmentType: new FormControl(),
      treatmentCost: new FormControl(),
      treatmentDate: new FormControl(),
      prescribedMedical: new FormControl(),
      treatmentNotes: new FormControl(),
      followUpReq: new FormControl(),
      followUpDate: new FormControl()
    });
  }

  initializeCreateForm() {
    this.patientTreatmentForm = new FormGroup({
      patient: new FormGroup({
        id: new FormControl(),
        firstName: new FormControl({ value: '', disabled: true }),
        email: new FormControl({ value: '', disabled: true }),
        dateOfBirth: new FormControl({ value: '', disabled: true }),
        gender: new FormControl({ value: '', disabled: true }),
        contactNo: new FormControl({ value: '', disabled: true }),
        address: new FormControl({ value: '', disabled: true })
      }),
      dr: new FormGroup({
        id: new FormControl(),
        userName: new FormControl({ value: '', disabled: true }),
        email: new FormControl({ value: '', disabled: true })
      }),
      dsa: new FormGroup({
        id: new FormControl(),
        userName: new FormControl({ value: '', disabled: true }),
        email: new FormControl({ value: '', disabled: true })
      }),
      treatmentNo: new FormControl({ value: '', disabled: true }),
      condition: new FormControl(),
      description: new FormControl(),
      treatmentType: new FormControl(),
      treatmentCost: new FormControl(),
      treatmentDate: new FormControl(),
      prescribedMedical: new FormControl(),
      treatmentNotes: new FormControl(),
      followUpReq: new FormControl(),
      followUpDate: new FormControl()
    });
  }

  getData() {
    const id = this.activeRoute.snapshot.paramMap.get('id');
    this.patientTreatmentForm.get('patient.dateOfBirth')?.enable();
    this.service.getPatientTreatmentFormById(Number(id)).subscribe({
      next: (result: PatientTreamentFormDto) => {
        if (result) {
          this.patientTreatmentForm.patchValue(result);
          this.patientTreatmentForm.get('patient')?.patchValue({
            dateOfBirth: new Date(result.patient.dateOfBirth)
          });
          this.patientTreatmentForm.patchValue({
            treatmentDate: result.treatmentDate ? new Date(result.treatmentDate) : null
          });
          this.patientTreatmentForm.get('patient.dateOfBirth')?.disable();
          this.patientTreatmentForm.get('treatmentDate')?.disable();
        }
      },
      error: (error) => {
        console.error('Error fetching patient treatment form:', error);
      }
    });
  }

  //TODO : Use API to add into table
  register() {

  }

}
