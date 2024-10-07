import { Component, ElementRef, Injectable, Output, ViewChild, EventEmitter, Input, SimpleChanges } from "@angular/core";
import { FormControl, FormGroup } from "@angular/forms";
import * as bootstrap from 'bootstrap';
import { FileUpload } from 'primeng/fileupload';
import { AppConsts } from "../../../shared/AppConsts";
import { Column, DepartmentDto, PositionDto, UserDto } from "../../../shared/model/AppModel";
import { UserRegister } from "../../../shared/model/user";
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

  @Output('callbackRecordCreate') eventEmitRecordCreate = new EventEmitter<any>();

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

  selectedDepartmentId: number = -1;
  //for text Area
  textVisible: boolean = false;
  textInput: string = "";
  textField: string = "";

  constructor(private services: userServices, private appconst: AppConsts) {
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
      userPhoto: new FormControl()
    })
  }

  getData(): void {
    if (this.userId != null) {
      this.services.getUserById(this.userId).subscribe({
        next: (result: UserDto) => {
          if (result) {
            this.userForm.patchValue(result);
            this.userForm.get("birthDate")?.patchValue(this.formatDate(result.birthDate));
          }
        }
      })
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
          { field: 'code', header: 'Code', type: 'string' },
          { field: 'name', header: 'Name', type: 'string' },
          { field: 'description', header: 'Description', type: 'string' }
        ];

        this.lookupTitle = "department";
        this.lookupTable = "department";
        this.lookupVisible = true;
      }
    })
  }

  loadPositionLookup() {
    this.services.getPositionByDeprtmId(this.selectedDepartmentId).subscribe({
      next: (result: PositionDto[]) => {
        if (result) {
          this.lookupData = result
        }

        let data = this.userForm.get('position')?.value
        if (data) { this.selectedLookupVal = this.lookupData.find(x => x.code == data) }

        this.lookupColumn = [
          { field: 'code', header: 'Code', type: 'string' },
          { field: 'name', header: 'Name', type: 'string' },
          { field: 'description', header: 'Description', type: 'string' }
        ];

        this.lookupTitle = "position";
        this.lookupTable = "position";
        this.lookupVisible = true;
      }
    })
  }

  isFormInvalid() {

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

  save() {

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
      let x = this.userForm.get('department')?.value;
      if (x != selectedLookup.code) {
        this.userForm.get('position')?.patchValue(null);
      }
      this.userForm.get('department')?.patchValue(selectedLookup.code);
      this.selectedDepartmentId = selectedLookup.id;
    }
    if (event.lookupTable == 'position') {
      this.selectedDepartmentId = -1;
      this.userForm.get('position')?.patchValue(selectedLookup.code);
    }
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
