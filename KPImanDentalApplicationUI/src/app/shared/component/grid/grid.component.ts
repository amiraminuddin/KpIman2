import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { LazyLoadEvent, MenuItem, SortEvent } from 'primeng/api';
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
  @Input() totalRecord: number = 0;

  @Output('callbackRecordSelected') eventEmitRecordSelected = new EventEmitter<any>();
  @Output('callbackActionTrigger') actionTriggered = new EventEmitter<{ action: MenuItem; rowData: any }>();
  @Output('callbackPageChange') eventEmitPageChange = new EventEmitter<any>();
  @Output('callbackSort') eventEmitSort = new EventEmitter<any>();

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


  paginate(event: any) {
    this.eventEmitPageChange.emit(event);
  }

  customSort(event: SortEvent) {
    let orderMode = "asc";
    if (event.order == -1) {
      orderMode = "desc"
    }
    this.eventEmitSort.emit({ field: event.field, mode: event.mode, orderByMode: orderMode })
  }

  private onActionTriggered(action: MenuItem, rowData: any) {
    this.actionTriggered.emit({ action, rowData });
  }

}
