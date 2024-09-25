import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from "@angular/core";
import { FormControl, FormGroup } from "@angular/forms";
import { DepartmentDto } from "../../../shared/model/AppModel";
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

  departmentForm = new FormGroup({});
  department: DepartmentDto = new DepartmentDto

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
  }

  getData(): void {
    if (this.departmentId != null) {
      this.service.getDepartmentById(this.departmentId).subscribe({
        next: (result: DepartmentDto) => {
          if (result) {
            this.departmentForm.patchValue(result);
            this.departmentForm.get('code')?.disable();
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
}
