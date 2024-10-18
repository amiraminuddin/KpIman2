import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { LazyLoadEvent, MenuItem } from 'primeng/api';
import { Column } from '../../../../shared/model/AppModel';

@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.css']
})
export class GridComponent implements OnInit {

  @Input() columns: Column[] = [];
  @Input() value: any[] = [];
  @Input() selectionMode: string | undefined;
  @Input() dataKey: string | undefined;
  @Input() actionList: MenuItem[] = [];

  @Output('callbackRecordSelected') eventEmitRecordSelected = new EventEmitter<any>();
  @Output('callbackActionTrigger') actionTriggered = new EventEmitter<{ action: MenuItem; rowData: any }>();

  selection: any;
  constructor() { }

  ngOnInit(): void {
  }

  onRowSelect(event: any) {
    this.eventEmitRecordSelected.emit(event);
  }

  getActions(rowData: any): MenuItem[] {
    return this.actionList.map(action => ({
      ...action,
      command: () => this.onActionTriggered(action, rowData)
    }));
  }

  private onActionTriggered(action: MenuItem, rowData: any) {
    this.actionTriggered.emit({ action, rowData });
  }

}