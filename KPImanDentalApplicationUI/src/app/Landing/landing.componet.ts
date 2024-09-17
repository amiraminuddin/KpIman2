import { Component } from "@angular/core";
import Swal from "sweetalert2";
import { ModuleService } from "../../shared/_services/module.service";

@Component({
  selector: 'app-landing-component',
  templateUrl: 'landing.component.html',
  styleUrls: ['landing.component.css']
})
export class LandingComponent {
  moduleData: any[] = []

  constructor(private moduleService: ModuleService) { }

  ngOnInit() {
    this.getAllModule();
  }

  getAllModule() {
    this.moduleService.GetModuleList().subscribe({
      next: response => {
        this.moduleData = response,
          this.moduleData.forEach((x) => {
            x.imageSource = `data:image/png;base64,${x.moduleIcon}`; // Format the image source
          }); },
      error: error => {
        Swal.fire({
          icon: "error",
          title: "Oops...",
          text: error.error
        });
      }
    })
  }

  getModuleIcon(code: string) {
    const module = this.moduleData.find(x => x.moduleName === code); // Corrected comparison
    return module
  }



  /* module.imageSource*/

}
