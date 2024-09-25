import { NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppComponent } from './app.component';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from '../shared/main/header-component';
import { FooterComponent } from '../shared/main/footer-component';
import { userComponent } from './users/user-list-component';
import { userModal } from './users/user-modal/user-modal-component';
import { LoginComponent } from './Account/login.component';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from '../app-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MainComponent } from '../shared/main/main-component';
import { UserConst } from '../shared/model/user';
import { MenuItem } from 'primeng/api';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LandingComponent } from './Landing/landing.componet';
import { SpinnerComponent } from '../shared/main/spinner-component';
import { AppConsts } from '../shared/AppConsts';
import { AuthInterceptor } from '../shared/_auth/auth_interceptor';
import { maintenanceComponent } from '../shared/main/maintenance-component';
import { ModuleComponent } from './Module/module.list.component';

import { FileUploadModule } from 'primeng/fileupload';
import { CheckboxModule } from 'primeng/checkbox';
import { CalendarModule } from 'primeng/calendar';
import { ImageModule } from 'primeng/image';
import { FieldsetModule } from 'primeng/fieldset';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { MenuModule } from 'primeng/menu';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { DividerModule } from "primeng/divider";
import { InputTextareaModule } from 'primeng/inputtextarea';
import { DialogModule } from 'primeng/dialog';
import { InputNumberModule } from 'primeng/inputnumber';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService } from 'primeng/api';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { TagModule } from 'primeng/tag';
import { BadgeModule } from 'primeng/badge';

import { ModuleModalComponent } from './Module/module.modal.component';
import { IconComponent } from '../shared/component/icon.component';
import { UserDetailComponent } from './users/user-detail-component';
import { UserMenu } from './users/User-Menu/user-menu-component';
import { DepartmentListComponent } from './users/Department/department-list-component';
import { positionComponent } from './users/Department/position-list-component';
import { PatientListComponent } from './Patient/PatientComponent/patient-list-component';
import { PatientTreatmentFormComponent } from './Patient/PatientTreatmentComponent/patient-treatment-form-component';
import { TreatmentListComponent } from './Lookup/Treatment/treamentL.list.component';
import { TreatmentLookupModalComponent } from './Lookup/Treatment/treatmentL.modal.component';
import { DepartmentModal } from './users/Department/department-modal.component';
import { PositionModalComponent } from './users/Department/position-modal-component';
import { PatientModalComponent } from './Patient/PatientComponent/patient-modal-component';



@NgModule({
  declarations: [
    AppComponent,
    MainComponent,
    HeaderComponent,
    FooterComponent,
    LandingComponent,
    userComponent,
    userModal,
    UserDetailComponent,
    LoginComponent,    
    ModuleComponent,
    ModuleModalComponent,
    DepartmentListComponent,
    positionComponent,
    DepartmentModal,
    PositionModalComponent,

    //patient Component
    PatientListComponent,
    PatientTreatmentFormComponent,
    PatientModalComponent,

    //lookup Component
    TreatmentListComponent,
    TreatmentLookupModalComponent,

    //Shared Component
    IconComponent,
    maintenanceComponent,
    SpinnerComponent,
    UserMenu,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    CommonModule,
    RouterModule,
    AppRoutingModule,
    FormsModule,

    //primeng module
    BrowserAnimationsModule,
    CalendarModule,
    FileUploadModule,
    CheckboxModule,
    ImageModule,
    FieldsetModule,
    InputTextModule,
    DropdownModule,
    MenuModule,
    TableModule,
    ToastModule,
    DividerModule,
    InputTextareaModule,
    DialogModule,
    InputNumberModule,
    ConfirmDialogModule,
    ProgressSpinnerModule,
    TagModule,
    BadgeModule,

    ReactiveFormsModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    UserConst,
    RouterModule,
    AppConsts,
    MessageService,
    ConfirmationService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }


