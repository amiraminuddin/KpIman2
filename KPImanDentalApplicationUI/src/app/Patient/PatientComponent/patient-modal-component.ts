import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from "@angular/core";
import { FormControl, FormGroup } from "@angular/forms";
import { AppConsts } from "../../../shared/AppConsts";
import { PatientDto } from "../../../shared/model/AppModel";
import { PatientService } from "../../../shared/_services/patient.service";

@Component({
  selector: 'patient-modal-component',
  templateUrl : 'patient-modal-component.html'
})

export class PatientModalComponent implements OnInit {

  @Input() patientId: number | null | undefined;
  @Input() modalVisible: boolean | undefined;
  @Input() modalState: string | undefined;

  @Output() refreshAfterSaved = new EventEmitter<any>();

  patientForm = new FormGroup({});
  patient: PatientDto = new PatientDto;
  genderList: any[] = []

  constructor(private service: PatientService, private appconst: AppConsts) {
    this.genderList = appconst.getGenderList();
  }

  ngOnInit(): void {
    this.initializeForm()
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes["modalVisible"] && this.modalVisible) {
      if (this.patientId != null) {
        this.getData();
      } else {
        this.resetForm();
      }
    } else {
      this.resetForm();
    }
  }

  initializeForm(): void{
    this.patientForm = new FormGroup({
      id: new FormControl(),
      firstName: new FormControl(),
      lastName: new FormControl(),
      email: new FormControl(),
      dateOfBirth: new FormControl(),
      gender: new FormControl(),
      contactNo: new FormControl(),
      address: new FormControl(),
      isActive: new FormControl()
    });
  }

  resetForm(): void {
    this.patientForm.reset();
    this.patientForm.get('isActive')?.setValue(true);
  }

  getData() {
    if (this.patientId != null) {
      this.service.getPatientsById(this.patientId).subscribe({
        next: (result: PatientDto) => {
          if (result) {
            this.patientForm.patchValue(result);
            this.patientForm.get('dateOfBirth')?.patchValue(this.formatDate(result.dateOfBirth));
          }
        },
        error: (error) => {
          console.log(error);
        }
      })
    }
  }

  save() {
    const formData = { ...this.patientForm.value }

    this.service.createOrUpdatePatient(formData).subscribe({
      next: _ => {
        this.refreshAfterSaved.emit(this.modalState);
      },
      error: (error) => { console.log(error) }
    });
  }

  private formatDate(date: any) {
    const d = new Date(date);
    let month = '' + (d.getMonth() + 1);
    let day = '' + d.getDate();
    const year = d.getFullYear();
    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;
    return [year, month, day].join('-');
  }
}
