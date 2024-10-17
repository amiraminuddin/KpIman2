import { Component } from "@angular/core";
import { AccountServices } from "../_services/account.service";
import { ActivatedRoute, NavigationEnd, Router } from "@angular/router";
import { filter } from "rxjs";
import { MenuItem } from "primeng/api";

@Component({
  selector: 'app-header-component',
  templateUrl: 'header-component.html'
})
export class HeaderComponent {

  userLogin: string | null = '';
  pageName: string | null = '';
  moduleName: string | null = '';

  menus: MenuItem[] = [];
  showMenu: boolean = true;


  constructor(
    private service: AccountServices,
    private router: Router,
    private activeRouter: ActivatedRoute,
  ) { }


  ngOnInit(): void {
    // Subscribe to router events to update header on navigation
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        this.updateHeader();
      });

    // Initial update in case the component loads on an already navigated route
    this.updateHeader();
    this.userLogin = this.service.getCurrentUserLogin();
  }

  logout() {
    this.service.isLogOut = true;
    this.service.logout();
    this.router.navigate(['']);
  }

  private updateHeader() {
    // Access the active route
    let currentRoute = this.activeRouter.root;

    // Traverse the route tree to find the relevant data
    while (currentRoute.children.length > 0) {
      currentRoute = currentRoute.children[0]; // Get the first child route
      if (currentRoute.outlet === 'primary') {
        // Check if the data is available
        this.moduleName = currentRoute.snapshot.data['moduleName'];
        this.pageName = currentRoute.snapshot.data['pageName'];
      }
    }

    if (this.moduleName == undefined || this.moduleName == 'Maintenance') {
      this.showMenu = false;
    } else {
      this.showMenu = true;
    }
  }

  getAction(moduleName: any) {
    if (moduleName == 'Configuration') {
      this.menus = [
        {
          label: 'Module Configuration',
          icon: 'pi pi-cog',
          command: () => {
            this.router.navigate(['/kpIman/Admin']);
          }
        },
        {
          label: 'Treatment Lookup',
          icon: 'pi pi-list',
          command: () => {
            this.router.navigate(['/kpIman/Admin/TreatmentLookup'])
          }
        }
      ];
    }
    if (moduleName == 'User Management') {
      this.menus = [
        {
          label: 'User List',
          icon: 'pi pi-user',
          command: () => {
            this.router.navigate(['/kpIman/UserManagement/user']);
          }
        },
        {
          label: 'Department List',
          icon: 'pi pi-building-columns',
          command: () => {
            this.router.navigate(['/kpIman/UserManagement/department']);
          },
        },
        {
          label: 'Organization Chart',
          icon: 'pi pi-user',
          command: () => {
            this.router.navigate(['/kpIman/UserManagement/OrgChart']);
          }
        }
      ];
    }
    if (moduleName == 'Patient Management') {
      this.menus = [
        {
          label: 'Patient List',
          icon: 'pi pi-user',
          command: () => {
            this.router.navigate(['/kpIman/Patient/PatientList']);
          }
        }
      ];
    }
    

    return this.menus;
  }

}
