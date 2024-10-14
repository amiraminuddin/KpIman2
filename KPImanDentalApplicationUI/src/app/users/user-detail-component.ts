import { HostListener } from "@angular/core";
import { Component, OnInit, ChangeDetectorRef, ElementRef, ViewChild } from "@angular/core";
import { Form, FormControl, FormGroup, NgForm, Validators } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";
import { MessageService } from "primeng/api";
import { timeout } from "rxjs";
import { isNullOrUndefined } from "util";
import { DepartmentDto, PositionDto, UserDto } from "../../shared/model/AppModel";
import { userServices } from "../../shared/_services/user.service";

interface Gender {
  name: string,
  code: string
}

@Component({
  selector: 'app-userDetail-component',
  templateUrl: 'user-detail-component.html',
  styleUrls: ['user-detail-component.css']
})
export class UserDetailComponent implements OnInit {

  @ViewChild('userForm') userForm?: NgForm;
  @HostListener('window:beforeunload', ['$event']) notify($event: any) {
    let x = true;
    this.userService.checkUserChange(this.user).toPromise().then(response => {
      if (response) {
        x = false;
      } else {
        x = true;
      }
    });
    if (x) {
      $event.returnValue = true;
    }
  }

  constructor(
    private userService: userServices,
    private activeRoute: ActivatedRoute,
    private changeDetector: ChangeDetectorRef,
    private messageService: MessageService
  ) { }

  user: UserDto = new UserDto();
  genders: Gender[] = [
    { name: 'Male', code: 'Male' },
    { name: 'Female', code: 'Female' }
  ];
  departments: DepartmentDto[] = [];
  positions: PositionDto[] = [];
  isDataChange: boolean = false;
  selectedFile: File | null = null;


  ngOnInit(): void {
    this.getData();
  }

  getData() {
    this.getUser();
    this.getDepartmentList();
    setTimeout(() => {
      this.setComponentHeight();
    }, 500); // Delay in milliseconds (3000 ms = 3 seconds)
  }

  getUser() {
    const userId = this.activeRoute.snapshot.paramMap.get('id');
    // Check if userId exists before calling the service
    if (userId) {
      this.userService.getUserById(Number(userId)).subscribe(result => {
        this.user = result;
        if (this.user) {
          //this.user.birthDate = new Date(this.user.birthDate);
          this.getDepartmentList();
          this.getPositionList(1);
        }
      })
    }
    this.changeDetector.detectChanges(); // Force change detection
  }

  getDepartmentList() {
    this.userService.getAllDepartment().subscribe({
      next: result => {
        this.departments = result;
      },
      error: error => console.log(error)
    });    
  }

  getPositionList(departmentId: number) {
    this.userService.getPositionByDeprtmId(departmentId).subscribe(result => {
      this.positions = result;
    });
    this.changeDetector.detectChanges();
  }

  onDepartmentChange(event: any) {
    if (event) {
      this.getPositionList(event.value);
    }     
  }

  onSelect(event: any) {

    this.selectedFile = event.currentFiles[0];

    if (this.selectedFile) {
      this.userService.saveUserPhoto(this.selectedFile).subscribe(
        (photoURL: string) => {
          this.user.userPhoto = photoURL;
          console.log('Photo uploaded successfully. URL:', photoURL);
        }
      )
    }
  }

  save() {
    //this.user.gender;
    //this.userService.updateUser(this.user).subscribe({
    //  next: user => {
    //    this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Data Saved!!' });
    //    this.getData();
    //  }
    //});
    //this.changeDetector.detectChanges(); // Force change detection
  }

  setComponentHeight() {
    var navbarElement = document.getElementById('MainHeader');
    if (navbarElement) {
      const navbarHeight = navbarElement.offsetHeight; // Safe to access
    }
  }

}
