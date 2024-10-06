import { Component, ElementRef, Injectable, Output, ViewChild, EventEmitter, Input, SimpleChanges } from "@angular/core";
import { FormControl, FormGroup } from "@angular/forms";
import * as bootstrap from 'bootstrap';
import { FileUpload } from 'primeng/fileupload';
import { AppConsts } from "../../../shared/AppConsts";
import { UserRegister } from "../../../shared/model/user";

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

  ngOnInit(): void {
    this.initializeForm();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes["modalVisible"] && this.modalVisible) {
      if (this.userId != null) {
        //this.getData();
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
    //getData User
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
}
