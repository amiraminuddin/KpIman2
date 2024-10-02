import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from "@angular/core";
import { FormControl, FormGroup } from "@angular/forms";
import { DataValidator, DepartmentDto, Validators, ValidatorTriggerType } from "../../../shared/model/AppModel";
import { userServices } from "../../../shared/_services/user.service";

@Component({
  selector: 'department-modal-component',
  templateUrl: 'department-modal.component.html'
})
export class DepartmentModal implements OnInit {

  @Input() departmentId: number | null | undefined;
  @Input() modalVisible: boolean | undefined;
  @Input() modalState: string | undefined;

  @Output() refreshAfterSaved = new EventEmitter<any>();

  public ValidatorTriggerType = ValidatorTriggerType;

  departmentForm = new FormGroup({});
  department: DepartmentDto = new DepartmentDto;
  validator: Validators[] = [];
  codeErrorMessage: string | null = null;
  dataValidator: DataValidator<DepartmentDto> = new DataValidator<DepartmentDto>()


  onChange = ValidatorTriggerType.OnChange;
  OnSave = ValidatorTriggerType.OnSave;
  OnLoad = ValidatorTriggerType.OnLoad;

  constructor(private service: userServices) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes["modalVisible"] && this.modalVisible) {
      if (this.departmentId != null) {
        this.getData();
      } else {
        this.resetForm();
        this.evaluateFieldFormula(ValidatorTriggerType.OnLoad);
      }
    } else if (changes['modalVisible'] && !this.modalVisible) {
      this.resetForm();
    }
  }

  initializeForm(): void {
    this.departmentForm = new FormGroup({
      id: new FormControl(),
      code: new FormControl(),
      name: new FormControl(),
      description: new FormControl()
    });
  }

  private resetForm(): void {
    this.departmentForm.reset();
    this.departmentForm.get('code')?.enable();
    this.validator = [];
  }

  getData(): void {
    if (this.departmentId != null) {
      this.service.getDepartmentById(this.departmentId).subscribe({
        next: (result: DepartmentDto) => {
          if (result) {
            this.departmentForm.patchValue(result);
            this.departmentForm.get('code')?.disable();
            this.evaluateFieldFormula(ValidatorTriggerType.OnLoad);
          }
        },
        error: (error) => {
          console.log(error);
        }
      });
    }
  }

  save() {
    const formData = { ...this.departmentForm.value };

    const departmentCode = this.departmentForm.get('code')?.value;
    formData.code = departmentCode;

    this.service.createOrUpdateDepartment(formData).subscribe({
      next: (result) => {
        this.refreshAfterSaved.emit(this.modalState);
      },
      error: (error) => {
        console.log(error);
      }
    })
  }


  //START : Validation
  evaluateFieldFormula(triggerType: ValidatorTriggerType): void {
    const formData = { ...this.departmentForm.value };
    const departmentCode = this.departmentForm.get('code')?.value;
    formData.code = departmentCode;

    this.dataValidator.data = formData;
    this.dataValidator.triggerType = triggerType

    this.service.getDepartmentValidator(this.dataValidator).subscribe({
      next: (result: Validators[]) => {
        this.validator = result;
      }
    });
  }

  isFormInvalid(): boolean {
    // If any validator in the list has isValid as false, the form is considered invalid
    return this.validator.some(x => !x.isValid);
  }
  //END : Validation
}
