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

  @Output() refreshAfterSaved = new EventEmitter<void>();

  treatmentForm = new FormGroup({});
  treatment: TreatmeantLookupDto = new TreatmeantLookupDto

  constructor(private service: LookupService) { }

  ngOnInit(): void {
    this.initializeForm();  
  }

  ngOnChanges(changes: SimpleChanges): void {
    // Only run this logic if the modal becomes visible and a treatmentId exists
    if (changes["modalVisible"] && this.modalVisible && this.treatmentId != null) {
      this.getData();
    }
  }

  initializeForm() : void {
    this.treatmentForm = new FormGroup({
      id: new FormControl(),
      treatmentCode: new FormControl(),
      treatmentName: new FormControl(),
      treatmentDesc: new FormControl(),
      isActive: new FormControl(false),
      treatmentPrice: new FormControl(),
    });
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
        this.refreshAfterSaved.emit();
      },
      error: (error) => { console.log(error) }
    });
  }
}
