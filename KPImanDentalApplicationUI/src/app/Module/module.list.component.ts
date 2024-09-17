import { Component, ElementRef, ViewChild } from "@angular/core";
import { DomSanitizer, SafeResourceUrl } from "@angular/platform-browser";
import Swal from "sweetalert2";
import { AppConsts } from "../../shared/AppConsts";
import { ModuleService } from "../../shared/_services/module.service";
import { ModuleModalComponent } from "./module.modal.component";

@Component({
  selector: 'app-module-component',
  templateUrl : 'module.list.component.html'
})
export class ModuleComponent {

  @ViewChild('moduleModal') moduleModal!: ModuleModalComponent;

  modules: any[]=[];
  isLoad: boolean = false;
  constructor(private moduleService: ModuleService, private appConst: AppConsts) { }

  ngOnInit() {
    this.getAllModule();
  }

  getAllModule() {
    this.isLoad = true;
    this.moduleService.GetModuleList().subscribe({
      next: response => {
        this.modules = response,
          this.modules.forEach((x) => {
            x.imageSource = `data:image/png;base64,${x.moduleIcon}`; // Format the image source
          });
        },
      error: error => {
        Swal.fire({
          icon: "error",
          title: "Oops...",
          text: error.error
        });
        this.isLoad = false;
      }
    });
    this.isLoad = false;    
  }

  editModule(moduleData: any) {
    this.moduleModal.show(this.appConst.getActionEditMode(), moduleData);
  }

  handleUpdateModule(data: any) {
    this.moduleService.UpdateModule(data).subscribe({
      next: response => this.getAllModule(),
      error: error => {
        Swal.fire({
          icon: "error",
          title: "Oops...",
          text: error.error
        });
      }
    })
  }
}
