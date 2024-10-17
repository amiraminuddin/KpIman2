import { Component, OnInit } from '@angular/core';
import { TreeNode } from 'primeng/api/treenode';
import { OrganizationChartDto } from '../../../shared/model/AppModel';
import { userServices } from '../../../shared/_services/user.service';

@Component({
  selector: 'app-organization',
  templateUrl: './organization.component.html',
  styleUrls: ['./organization.component.css']
})
export class OrganizationComponent implements OnInit {

  constructor(private service: userServices) { }
  data1: TreeNode[] = [];

  ngOnInit(): void {
    this.service.getOrganizationChart().subscribe({
      next: (result: TreeNode[]) => {
        if (result) {
          this.data1 = result;
        }        
      }
    })
  }
}