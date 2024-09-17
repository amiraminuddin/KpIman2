import { Injectable, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './app/Account/login.component';
import { LandingComponent } from './app/Landing/landing.componet';
import { ModuleComponent } from './app/Module/module.list.component';
import { DepartmentListComponent } from './app/users/Department/department-list-component';
import { UserDetailComponent } from './app/users/user-detail-component';
import { userComponent } from './app/users/user-list-component';
import { MainComponent } from './shared/main/main-component';
import { maintenanceComponent } from './shared/main/maintenance-component';
import { authGuard } from './shared/_routeGuard/auth.guard';
import { logoutGuard } from './shared/_routeGuard/logout.guard';
import { ModuleGuard } from './shared/_routeGuard/module.guard';
import { UserDetailUnsavedGuard } from './shared/_routeGuard/user.detail.unsaved.guard';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent, canActivate: [logoutGuard], runGuardsAndResolvers: 'always' },
  {
    path: 'kpIman', component: MainComponent, canActivate: [authGuard], runGuardsAndResolvers: 'always',
    children: [
      { path: '', redirectTo: 'main', pathMatch: 'full' },
      { path: 'main', component: LandingComponent },

      { path: 'UserManagement/user', component: userComponent, data: { moduleName: 'User Management', pageName: '> User List' } },
      {
        path: 'UserManagement/user/:id', component: UserDetailComponent, canDeactivate: [UserDetailUnsavedGuard],
        data: { moduleName: 'User Management', pageName: '> User Details' }
      },

      { path: 'UserManagement/department', component: DepartmentListComponent, data: { moduleName: 'User Management', pageName: '> Department List' } },

      { path: 'Admin', component: ModuleComponent, data: { moduleName: 'Configuration', pageName: '> Module List' } },

      { path: 'maintenance', component: maintenanceComponent, data: {moduleName: 'Maintenance', pageName: ''} },
    ]
  },
  { path: '**', component: MainComponent}
  // Other routes go here
];

@Injectable({
  providedIn: 'root'
})

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }