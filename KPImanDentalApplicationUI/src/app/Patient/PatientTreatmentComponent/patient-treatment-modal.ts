import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from "@angular/core";
import { FormControl, FormGroup } from "@angular/forms";
import { Column, PatientTreatmentDto, PatientTreatmentDtoExt, SelectedLookup, StaffLookupDto, TreatmeantLookupDto } from "../../../shared/model/AppModel";
import { LookupService } from "../../../shared/_services/lookup.service";
import { PatientService } from "../../../shared/_services/patient.service";

@Component({
  selector: 'patient-treatment-modal',
  templateUrl: 'patient-treatment-modal.html'
})

export class PatientTreatmentModal implements OnInit {

  @Input() treatmentId: number | null | undefined;
  @Input() patientId: number | null | undefined;
  @Input() modalVisible: boolean | undefined;
  @Input() modalState: string | undefined;

  @Output('callbackRecordSaved') eventEmitRecordSave = new EventEmitter<any>();

  patientTreatmentForm = new FormGroup({})

  //for lookup
  lookupTitle: string = "";
  lookupData: any[] = [];
  lookupColumn!: Column[];
  lookupVisible: boolean = false;
  lookupTable: string = "";

  selectedLookupVal: any;

  //for text Area
  textVisible: boolean = false;
  textInput: string = "";
  textField: string = "";
  constructor(private patientService: PatientService, private lookupService: LookupService) {

  }

  ngOnInit(): void {
    this.InitializeForm();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['modalVisible'] && this.modalVisible) {
      if (this.treatmentId != null) {
        this.getData();
        this.resetForm();
      } else {
        this.resetForm();
        this.getData();
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
      treatmentCost: new FormControl(),
      treatmentDate: new FormControl(),
      prescribedMedical: new FormControl(),
      treatmentNotes: new FormControl(),
      followUpReq: new FormControl(),
      followUpDate: new FormControl(),
      //lookup to Display
      treatmentDisplayValue: new FormControl(),
      patientDisplayValue: new FormControl(),
      drDisplayValue: new FormControl(),
      dsaDisplayValue: new FormControl()
    })
  }

  resetForm(): void {
    this.patientTreatmentForm.reset();
    this.patientTreatmentForm.get('treatmentNo')?.disable();
    this.patientTreatmentForm.get('treatmentDisplayValue')?.disable();
    this.patientTreatmentForm.get('patientDisplayValue')?.disable();
    this.patientTreatmentForm.get('drDisplayValue')?.disable();
    this.patientTreatmentForm.get('dsaDisplayValue')?.disable();
  }

  getData() {
    if (this.treatmentId != null) {
      this.patientService.getPatientTreatmentFormById(this.treatmentId).subscribe({
        next: (result: PatientTreatmentDtoExt) => {
          if (result) {
            this.patientTreatmentForm.patchValue(result);
            this.patientTreatmentForm.get('treatmentDisplayValue')?.patchValue(result.treatment?.fieldDisplay);
            this.patientTreatmentForm.get('patientDisplayValue')?.patchValue(result.patientName);
            this.patientTreatmentForm.get('drDisplayValue')?.patchValue(result.doctor?.fieldDisplay);
            this.patientTreatmentForm.get('dsaDisplayValue')?.patchValue(result.dsa?.fieldDisplay);
            this.patientTreatmentForm.get('treatmentDate')?.patchValue(this.formatDate(result.treatmentDate));
            if (result.followUpDate != null) this.patientTreatmentForm.get('followUpDate')?.patchValue(this.formatDate(result.followUpDate));
            //this.patientTreatmentForm.get('treatmentDisplayValue')?.disable();
          }
        }
      })
    } else {
      if (this.patientId != null) {
        this.patientService.getPatientsById(this.patientId).subscribe({
          next: (result: any) => {
            const currentDate = new Date();
            this.patientTreatmentForm.get('treatmentDate')?.patchValue(this.formatDate(currentDate));
            this.patientTreatmentForm.get('patientId')?.patchValue(result.id);
            this.patientTreatmentForm.get('patientDisplayValue')?.patchValue(result.firstName);
            this.patientTreatmentForm.get('followUpReq')?.patchValue(false);
          }
        })
      }
    }    
  }

  save() {
    const formData = { ...this.patientTreatmentForm.value }

    const treatmentfollowUpDate = this.patientTreatmentForm.get('followUpDate')?.value;
    const treatmentNoValue = this.patientTreatmentForm.get('treatmentNo')?.value;

    formData.treatmentNo = treatmentNoValue;
    if (treatmentfollowUpDate == null) formData.followUpDate = null;

    this.patientService.createOrUpdatePatientTreatment(formData).subscribe({
      next: _ => {
        this.eventEmitRecordSave.emit(this.modalState);
      }
    });
  }

  loadLookupTreatment() {
    this.lookupService.getAllTreatment().subscribe({
      next: (result: TreatmeantLookupDto[]) => {
        if (result) {
          this.lookupData = result;
        }

        if (this.patientTreatmentForm.get('treatmentType')?.value) {
          this.selectedLookupVal = this.lookupData.find(x => x.id == this.patientTreatmentForm.get('treatmentType')?.value)
        }

        this.lookupColumn = [
          { field: 'treatmentCode', header: 'Code', type: 'string', sortable: false },
          { field: 'treatmentName', header: 'Name', type: 'string', sortable: false },
          { field: 'treatmentDesc', header: 'Desc', type: 'string', sortable: false },
          { field: 'isActive', header: 'Is Active', type: 'bool', sortable: false },
          { field: 'treatmentPrice', header: 'Price', type: 'string', sortable: false }
        ];

        this.lookupTitle = 'Treatment';
        this.lookupTable = 'Treatment';
        this.lookupVisible = true;
      }
    })
  }

  loadUserTreatment(position: string) {
    this.lookupService.getUserLookup(position).subscribe({
      next: (result: StaffLookupDto[]) => {
        if(result){
          this.lookupData = result
        }

        if (position == 'Dr') {
          let data = this.patientTreatmentForm.get('drID')?.value
          if (data) { this.selectedLookupVal = this.lookupData.find(x => x.id == data) }
        }

        if (position == 'DSA') {
          let data = this.patientTreatmentForm.get('dsaId')?.value
          if (data) { this.selectedLookupVal = this.lookupData.find(x => x.id == data) }
        }

        this.lookupColumn = [
          { field: 'userName', header: 'Name', type: 'string', sortable: false },
          { field: 'email', header: 'Email', type: 'string', sortable: false },
          { field: 'position', header: 'Position', type: 'string', sortable: false },
          { field: 'department', header: 'Department', type: 'string', sortable: false },
        ];

        this.lookupTitle = position;
        this.lookupTable = position;
        this.lookupVisible = true;
      }
    })
  }

  loadTextArea(field: any) {
    var input = "";
    if (field = 'description') {
      input = this.patientTreatmentForm.get('description')?.value;
    }
    this.textField = field;
    this.textInput = input;
    this.textVisible = true;
  }

  getSelectedLookup(event: any) {
    let selectedLookup = event.data;
    if (event.lookupTable == 'Treatment') {
      this.patientTreatmentForm.get('treatmentType')?.patchValue(selectedLookup.id);
      this.patientTreatmentForm.get('treatmentDisplayValue')?.patchValue(selectedLookup.treatmentName);
      this.patientTreatmentForm.get('treatmentCost')?.patchValue(selectedLookup.treatmentPrice);
    }

    if (event.lookupTable == 'Dr') {
      this.patientTreatmentForm.get('drID')?.patchValue(selectedLookup.id);
      this.patientTreatmentForm.get('drDisplayValue')?.patchValue(selectedLookup.userName);
    }

    if (event.lookupTable == 'DSA') {
      this.patientTreatmentForm.get('dsaId')?.patchValue(selectedLookup.id);
      this.patientTreatmentForm.get('dsaDisplayValue')?.patchValue(selectedLookup.userName);
    }

    this.lookupVisible = false;
  }

  getTextData(event: any) {
    if (event.textField == 'description') {
      this.patientTreatmentForm.get('description')?.patchValue(event.data);
    }    
    this.textVisible = false;
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
