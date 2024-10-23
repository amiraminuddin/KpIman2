import { Component, Output, ViewChild, EventEmitter, Input, SimpleChanges } from "@angular/core";
import { FormControl, FormGroup } from "@angular/forms";
import * as bootstrap from 'bootstrap';
import { FileUpload } from 'primeng/fileupload';
import { AppConsts } from "../../../shared/AppConsts";
import { Column, DataValidator, DepartmentDto, PositionDto, StaffLookupDto, UserCreateDto, UserDto, UserDtoExt, Validators, ValidatorTriggerType } from "../../../shared/model/AppModel";
import { UserRegister } from "../../../shared/model/user";
import { LookupService } from "../../../shared/_services/lookup.service";
import { userServices } from "../../../shared/_services/user.service";

interface UploadEvent {
  originalEvent: Event;
  files: File[];
}

@Component({
  selector: 'app-user-modal',
  templateUrl: 'user-modal-component.html',
})
export class userModal {
  @ViewChild('fileUpload')
    fileUpload!: FileUpload;

  @Input() userId: number | null | undefined;
  @Input() modalVisible: boolean | undefined;
  @Input() modalState: string | undefined;

  @Output('callbackRefreshAfterSaved') refreshAfterSaved = new EventEmitter<any>();

  userForm = new FormGroup({});

  user = new UserRegister;
  modalInstance: bootstrap.Modal | undefined;
  currentMode: string = "";

  genderList: any[] = []

  //for lookup
  lookupTitle: string = "";
  lookupData: any[] = [];
  lookupColumn!: Column[];
  lookupVisible: boolean = false;
  lookupTable: string = "";

  selectedLookupVal: any;
  selectedDepartmentId: number | undefined;

  //for text Area
  textVisible: boolean = false;
  textInput: string = "";
  textField: string = "";

  departmentList: DepartmentDto[] = [];

  //Validator
  validator: Validators[] = [];
  dataValidator: DataValidator<UserCreateDto> = new DataValidator<UserCreateDto>()
  public ValidatorTriggerType = ValidatorTriggerType;
  onChange = ValidatorTriggerType.OnChange;
  OnSave = ValidatorTriggerType.OnSave;
  OnLoad = ValidatorTriggerType.OnLoad;

  showhierarchyLevelField: boolean = false;

  constructor(private services: userServices, private appconst: AppConsts, private lookupServices: LookupService) {
    this.genderList = appconst.getGenderList();
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes["modalVisible"] && this.modalVisible) {
      if (this.userId != null) {
        this.getData();
      } else {
        this.resetForm();
        this.userForm.get('isActive')?.patchValue(true);
        this.userForm.get('isSupervisor')?.patchValue(false);
        this.evaluateFieldFormula(ValidatorTriggerType.OnLoad);
      }
    } else {
      this.resetForm();
    }
    
  }

  private resetForm(): void {
    this.userForm.reset();
  }

  initializeForm(): void {
    this.userForm = new FormGroup({
      id: new FormControl(),
      userName: new FormControl(),
      fullName: new FormControl(),
      email: new FormControl(),
      password: new FormControl(),
      position: new FormControl(),
      department: new FormControl(),
      birthDate: new FormControl(),
      address: new FormControl(),
      isActive: new FormControl(),
      isSupervisor: new FormControl(),
      supervisorId: new FormControl(),
      gender: new FormControl(),
      userPhoto: new FormControl(),
      positionL: new FormGroup({
        fieldValue: new FormControl(),
        fieldDisplay: new FormControl(),
      }),
      departmentL: new FormGroup({
        fieldValue: new FormControl(),
        fieldDisplay: new FormControl(),
      }),
      supervisorNameL: new FormGroup({
        fieldValue: new FormControl(),
        fieldDisplay: new FormControl(),
      }),
      confirmPassword: new FormControl(),
      hierarchyLevel: new FormControl()
    })
  }

  getData(): void {
    if (this.userId != null) {

      this.services.getAllDepartment().subscribe({
        next: (result: DepartmentDto[]) => {
          if (result) {
            this.departmentList = result;
          }
        }
      });

      this.services.GetUserForEdit(this.userId).subscribe({
        next: (result: UserCreateDto) => {
          if (result) {
            this.userForm.patchValue(result);
            this.userForm.get("birthDate")?.patchValue(this.formatDate(result.birthDate));
            const department = this.departmentList.find(x => x.code == result.department);
            this.selectedDepartmentId = department ? department.id : undefined;
            this.evaluateFieldFormula(ValidatorTriggerType.OnLoad);
          }
        }
      });


    }
  }

  loadTextArea(field: any) {
    var input = "";
    if (field = 'address') {
      input = this.userForm.get('address')?.value;
    }
    this.textField = field;
    this.textInput = input;
    this.textVisible = true;
  }

  loadDepartmentLookup() {
    this.services.getAllDepartment().subscribe({
      next: (result: DepartmentDto[]) => {
        if (result) {
          this.lookupData = result
        }

        let data = this.userForm.get('department')?.value
        if (data) { this.selectedLookupVal = this.lookupData.find(x => x.code == data) }

        this.lookupColumn = [
          { field: 'code', header: 'Code', type: 'string', sortable: false },
          { field: 'name', header: 'Name', type: 'string', sortable: false },
          { field: 'description', header: 'Description', type: 'string', sortable: false }
        ];

        this.lookupTitle = "department";
        this.lookupTable = "department";
        this.lookupVisible = true;
      }
    })
  }

  loadPositionLookup() {
    if (this.selectedDepartmentId != undefined)
    this.services.getPositionByDeprtmId(this.selectedDepartmentId).subscribe({
      next: (result: PositionDto[]) => {
        if (result) {
          this.lookupData = result
        }

        let data = this.userForm.get('position')?.value
        if (data) { this.selectedLookupVal = this.lookupData.find(x => x.code == data) }

        this.lookupColumn = [
          { field: 'code', header: 'Code', type: 'string', sortable: false },
          { field: 'name', header: 'Name', type: 'string', sortable: false },
          { field: 'description', header: 'Description', type: 'string', sortable: false }
        ];

        this.lookupTitle = "position";
        this.lookupTable = "position";
        this.lookupVisible = true;
      }
    })
  }

  loadSvUserLookup(hierachyLevel: number) {
    this.lookupServices.getUserLookupByHierachyLevel(hierachyLevel).subscribe({
      next: (result: StaffLookupDto[]) => {
        if (result) {
          this.lookupData = result;
        }

        let data = this.userForm.get('supervisorId')?.value;
        if (data) { this.selectedLookupVal = this.lookupData.find(x => x.id == data) }

        this.lookupColumn = [
          { field: 'fullName', header: 'Name', type: 'string', sortable: false },
          { field: 'email', header: 'Email', type: 'string', sortable: false },
          { field: 'departmentL', header: 'Department', type: 'lookup', sortable: false },
          { field: 'positionL', header: 'Position', type: 'lookup', sortable: false },
        ];

        this.lookupTitle = "User List";
        this.lookupTable = "User";
        this.lookupVisible = true;
      }
    })
  }

  onSelect(event: any) {
    // Handle the upload event
    console.log('File uploaded:', event);
    const img = event.files[0];
    var based64 = "";
    if (img) {
      const reader = new FileReader();
      for (let file of event.files) {
        reader.readAsDataURL(file);
        reader.onload = () => {
          const base64 = reader.result?.toString() || ''; // Get the Base64 string
          // Patch the value after the file is loaded
          this.userForm.get('userPhoto')?.patchValue(base64);
          // Log for debugging
          console.log('Base64:', base64);
        };
      }      
    }

    // Clear the file input after upload
    this.fileUpload.clear();
  }

  onHierarchyLevelChange() {
    this.userForm.get('supervisorId')?.patchValue(null);
    this.userForm.get('supervisorNameL')?.reset({
      fieldValue: null,
      fieldDisplay: null
    });
  }

  save() {
    const formData = { ...this.userForm.value };

    formData.isActive = formData.isActive === 'true' || formData.isActive === true;

    this.services.CreateOrUpdateUser(formData).subscribe({
      next: (result) => {
        console.log(result)
        this.refreshAfterSaved.emit(this.modalState);
      },
      error: (error) => { console.log(error) }
    });
  }

  getTextData(event: any) {
    if (event.textField == 'address') {
      this.userForm.get('address')?.patchValue(event.data);
    }
    this.textVisible = false;
  }

  getSelectedLookup(event: any) {
    let selectedLookup = event.data;
    if (event.lookupTable == 'department') {
      let lookupData = { fieldValue: selectedLookup.code, fieldDisplay: selectedLookup.name }
      let x = this.userForm.get('department')?.value;
      if (x != selectedLookup.code) {
        this.userForm.get('position')?.patchValue(null);
        this.userForm.get('positionL')?.reset({
          fieldValue: null,
          fieldDisplay: null
        });
      }
      this.userForm.get('department')?.patchValue(selectedLookup.code);
      this.userForm.get('departmentL')?.patchValue(lookupData);
      this.selectedDepartmentId = selectedLookup.id;
    }
    if (event.lookupTable == 'position') {
      let lookupData = { fieldValue: selectedLookup.code, fieldDisplay: selectedLookup.name }
      this.selectedDepartmentId = -1;
      this.userForm.get('position')?.patchValue(selectedLookup.code);
      this.userForm.get('positionL')?.patchValue(lookupData);
    }
    if (event.lookupTable == 'User') {
      let lookupData = { fieldValue: String(selectedLookup.id), fieldDisplay: selectedLookup.fullName }
      this.userForm.get('supervisorId')?.patchValue(Number(selectedLookup.id));
      this.userForm.get('supervisorNameL')?.patchValue(lookupData);
    }
    this.lookupVisible = false;
    this.evaluateFieldFormula(ValidatorTriggerType.OnChange);
  }

  checkHierarchyLevel() {
    const hierarchyLevel = this.userForm.get('hierarchyLevel')?.value;
    this.showhierarchyLevelField = hierarchyLevel > 1;
  }

  evaluateFieldFormula(triggerType: ValidatorTriggerType): void {
    const formData = { ...this.userForm.value };
    //const departmentCode = this.userForm.get('code')?.value;
    formData.isActive = this.userForm.get('isActive')?.value;
    formData.isSupervisor = this.userForm.get('isSupervisor')?.value;

    this.dataValidator.data = formData;
    this.dataValidator.triggerType = triggerType

    this.services.getUserValidator(this.dataValidator).subscribe({
      next: (result: Validators[]) => {
        this.validator = result;
      }
    });
  }

  isFormInvalid(): boolean {
    // If any validator in the list has isValid as false, the form is considered invalid
    return this.validator.some(x => !x.isValid);
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
