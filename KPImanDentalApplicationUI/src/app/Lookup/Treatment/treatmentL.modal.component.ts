import { Component, Input, OnInit, OnChanges, SimpleChanges, Output, EventEmitter } from "@angular/core";
import { FormControl, FormGroup } from "@angular/forms";
import { TreatmeantLookupDto } from "../../../shared/model/AppModel";
import { LookupService } from "../../../shared/_services/lookup.service";


@Component({
  selector: 'treatment-lookup-modal',
  templateUrl: 'treatmentL.modal.component.html',
  styleUrls: ['treatmentL.modal.css']
})
export class TreatmentLookupModalComponent implements OnInit {

  @Input() treatmentId: number | null | undefined;
  @Input() modalVisible: boolean | undefined;
  @Input() modalState: string | undefined;

  @Output() refreshAfterSaved = new EventEmitter<any>();

  treatmentForm = new FormGroup({});
  treatment: TreatmeantLookupDto = new TreatmeantLookupDto

  constructor(private service: LookupService) { }

  ngOnInit(): void {
    this.initializeForm();  
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes["modalVisible"] && this.modalVisible) {
      // When the modal becomes visible, check treatmentId
      if (this.treatmentId != null) {
        this.getData();
      } else {
        this.resetForm(); // Reset the form if no treatmentId
        //this.treatmentForm.get('treatmentCode')?.enable(); // Enable treatmentCode
      }
    } else if (changes["modalVisible"] && !this.modalVisible) {
      this.resetForm(); // Reset when modal is hidden
    }
  }

  initializeForm() : void {
    this.treatmentForm = new FormGroup({
      id: new FormControl(),
      treatmentCode: new FormControl(),
      treatmentName: new FormControl(),
      treatmentDesc: new FormControl(),
      isActive: new FormControl(),
      treatmentPrice: new FormControl(),
    });
  }

  private resetForm(): void {
    this.treatmentForm.reset(); // Reset the form values
    this.treatmentForm.get('treatmentCode')?.enable(); // Enable treatmentCode after reset
  }

  getData(): void {
    if (this.treatmentId != null) {
      let id = this.treatmentId;
      this.service.getTreatmentById(id).subscribe({
        next: (result: TreatmeantLookupDto) => {
          if (result) {
            this.treatmentForm.patchValue(result);
            this.treatmentForm.get('treatmentCode')?.disable();
          }
        },
        error: (error) => {
          console.log('Error fetching');
        }
      });
    }    
  }

  save() {
    const formData = { ...this.treatmentForm.value };

    const treatmentCodeValue = this.treatmentForm.get('treatmentCode')?.value;
    formData.treatmentCode = treatmentCodeValue;

    formData.isActive = formData.isActive === 'true' || formData.isActive === true;
    this.service.createOrUpdateTreatmentLookup(formData).subscribe({
      next: (result) => {
        console.log(result)
        this.refreshAfterSaved.emit(this.modalState);
      },
      error: (error) => { console.log(error) }
    });
  }
}
