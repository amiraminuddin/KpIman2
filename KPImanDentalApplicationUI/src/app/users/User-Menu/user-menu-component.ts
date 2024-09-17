import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { MenuItem } from "primeng/api";

@Component({
  selector: 'user-menu-component',
  templateUrl: 'user-menu-component.html'
})

export class UserMenu implements OnInit {
  items: MenuItem[] = [];

  constructor(private router : Router) { }

  ngOnInit() {
    this.items = [
      {
        label: 'Navigate',
        items: [
          {
            label: 'User List',
            icon: 'pi pi-users',
            command: () => {
              this.router.navigate(['/kpIman/UserManagement/user']);
            }
          },
          {
            label: 'Department',
            icon: 'pi pi-building-columns',
            command: () => {
              this.router.navigate(['/kpIman/UserManagement/department']);
            }
          },
          {
            label: 'Position',
            icon: 'pi pi-crown',
            command: () => {
              this.router.navigate(['/kpIman/UserManagement/user']);
            }
          }
        ]
      }
    ];
  }
}
