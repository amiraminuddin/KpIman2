import { Component, ElementRef, Output, ViewChild, EventEmitter } from "@angular/core";
import * as bootstrap from "bootstrap";
import { AppConsts } from "../../shared/AppConsts";
import { ModuleUpdateDto } from "../../shared/model/AppModel";

interface UploadEvent {
  originalEvent: Event;
  files: File[];
}

@Component({
  selector: 'module-modal-component',
  templateUrl: 'module.modal.component.html'
})
export class ModuleModalComponent {

  @ViewChild('moduleModal') moduleModal!: ElementRef;
  @Output('callbackRecordCreate') eventEmitRecordCreate = new EventEmitter<any>();

  constructor(public _appConsts: AppConsts) { }

  modalInstance: bootstrap.Modal | undefined;
  currentMode: string = "";
  module = new ModuleUpdateDto;
  moduleIconStr: string = "";


  ngAfterViewInit() {
    const modalElement = this.moduleModal.nativeElement;
    this.modalInstance = new bootstrap.Modal(modalElement);
  }

  //always edit
  show(mode: string, moduleData: any) {
    this.currentMode = mode;
    if (moduleData != null) {
      this.module = moduleData;
    } else {
      this.module = new ModuleUpdateDto;
    }
    this.modalInstance?.show();
  }

  save() {
    this.eventEmitRecordCreate.emit(this.module);
    this.modalInstance?.hide();
  }

  onUpload(event: any) {
  }

  onSelect(event: any) {
    const file = event.files[0]; // Get the first selected file
    if (file) {
      const reader = new FileReader();
      reader.readAsDataURL(file); // Convert file to Base64 string

      reader.onload = () => {
        this.moduleIconStr = reader.result as string; // Store Base64 string

        // Call API to save the Base64 string to the database
        this.module.moduleIcon = this.getBase64(this.moduleIconStr);
      };

      reader.onerror = (error) => {
      };
    }
  }

  getBase64(base64String: string) {
    // Remove the prefix 'data:image/png;base64,' or similar from the Base64 string
    return base64String.split(',')[1];
  }




}
