import { Component, Input, OnInit, Output, SimpleChanges, EventEmitter } from "@angular/core";
import { FormControl, FormGroup } from "@angular/forms";
import { PositionDto } from "../../../shared/model/AppModel";
import { userServices } from "../../../shared/_services/user.service";


@Component({
  selector: 'position-modal-component',
  templateUrl: 'position-modal-component.html'
})
export class PositionModalComponent implements OnInit {

  @Input() positionId: number | null | undefined;
  @Input() departmentId: number | null | undefined;

  @Input() modalVisible: boolean | undefined;
  @Input() modalState: string | undefined;

  @Output() refreshAfterSaved = new EventEmitter<any>();

  positionForm = new FormGroup({})


  constructor(private services: userServices) { }

  ngOnInit(): void {
    this.initilizeForm();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes["modalVisible"] && this.modalVisible) {
      if (this.positionId != null) {
        this.getData();
      }
      else {
        this.resetForm();
      }
    } else if (changes["modalVisible"] && !this.modalVisible) {
      this.resetForm();
    }
  }

  initilizeForm(): void {
    this.positionForm = new FormGroup({
      id: new FormControl(),
      departmentId: new FormControl(),
      //departmentName: new FormControl(),
      code: new FormControl(),
      name: new FormControl(),
      description: new FormControl()
    })
  }

  resetForm(): void {
    this.positionForm.reset();
    this.positionForm.get('code')?.enable();
  }


  getData(): void {
    if (this.positionId) {
      this.services.getPositionById(this.positionId).subscribe({
        next: (result: PositionDto) => {
          if (result) {
            this.positionForm.patchValue(result);
            this.positionForm.get('code')?.disable();
          }
        },
        error: (error) => {
          console.log(error);
        }
      });
    }
  }


  save() {
    const formData = { ...this.positionForm.value };

    const positionCode = this.positionForm.get('code')?.value;
    formData.code = positionCode
    formData.departmentId = this.departmentId

    this.services.createOrUpdatePosition(formData).subscribe({
      next: (result) => {
        this.refreshAfterSaved.emit(this.modalState);
      },
      error: (error) => {
        console.log(error);
      }
    })
  }

}
