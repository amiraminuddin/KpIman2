import { Component, Input, OnInit, SimpleChanges } from "@angular/core";
import { FormControl, FormGroup } from "@angular/forms";
import { Column, PatientTreatmentDto, SelectedLookup, TreatmeantLookupDto } from "../../../shared/model/AppModel";
import { LookupService } from "../../../shared/_services/lookup.service";
import { PatientService } from "../../../shared/_services/patient.service";

@Component({
  selector: 'patient-treatment-modal',
  templateUrl: 'patient-treatment-modal.html'
})

export class PatientTreatmentModal implements OnInit {

  @Input() treatmentId: number | null | undefined;
  @Input() modalVisible: boolean | undefined;
  @Input() modalState: string | undefined;

  patientTreatmentForm = new FormGroup({})

  //for lookup
  lookupTitle: string = "";
  lookupData: any[] = [];
  lookupColumn!: Column[];
  lookupVisible: boolean = false;

  selectedTreatment!: SelectedLookup[];


  constructor(private patientService: PatientService, private lookupService: LookupService) {

  }

  ngOnInit(): void {
    this.InitializeForm();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['modalVisible'] && this.modalVisible) {
      if (this.treatmentId != null) {
        this.getData();
      } else {
        this.resetForm();
      }
    } else {
      this.resetForm();
    }
  }


  InitializeForm(): void {
    this.patientTreatmentForm = new FormGroup({
      id: new FormControl(),
      patientId: new FormControl(),
      drID: new FormControl(),
      dsaId: new FormControl(),
      treatmentNo: new FormControl(),
      condition: new FormControl(),
      description: new FormControl(),
      treatmentType: new FormControl(),
      treatmentDisplayValue: new FormControl(),
      treatmentCost: new FormControl(),
      treatmentDate: new FormControl(),
      prescribedMedical: new FormControl(),
      treatmentNotes: new FormControl(),
      followUpReq: new FormControl(),
      followUpDate: new FormControl()
    })
  }

  resetForm(): void {
    this.patientTreatmentForm.reset();
    this.patientTreatmentForm.get('treatmentDisplayValue')?.disable();
  }

  getData() {
    if (this.treatmentId != null) {
      this.patientService.getPatientTreatmentFormById(this.treatmentId).subscribe({
        next: (result: PatientTreatmentDto) => {
          if (result) {
            this.patientTreatmentForm.patchValue(result);
            this.patientTreatmentForm.get('treatmentDate')?.patchValue(this.formatDate(result.treatmentDate));
            this.patientTreatmentForm.get('treatmentDisplayValue')?.disable();
          }
        }
      })
    }    
  }

  save() {
    console.log(this.patientTreatmentForm.value);
  }

  loadLookupTreatment() {
    this.lookupService.getAllTreatment().subscribe({
      next: (result: TreatmeantLookupDto[]) => {
        if (result) {
          this.lookupData = result;
        }

        this.lookupColumn = [
          { field: 'treatmentCode', header: 'Code' },
          { field: 'treatmentName', header: 'Name' },
          { field: 'treatmentDesc', header: 'Desc' },
          { field: 'isActive', header: 'Is Active' },
          { field: 'treatmentPrice', header: 'Price' }
        ];

        this.lookupTitle = 'Treatment';
        this.lookupVisible = true;
      }
    })
  }

  getSelectedLookup(event: any) {
    console.log(event);
    this.selectedTreatment = [{ value: event.Id, displayValue: event.treatmentName }];
    this.patientTreatmentForm.get('treatmentType')?.patchValue(this.selectedTreatment[0].value);
    this.patientTreatmentForm.get('treatmentDisplayValue')?.patchValue(this.selectedTreatment[0].displayValue);
    this.lookupVisible = false;
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
