import { ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MenuItem, SortEvent } from 'primeng/api';
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
  @Input() filterLegend: string = "";

  @Output('callbackRecordSelected') eventEmitRecordSelected = new EventEmitter<any>();
  @Output('callbackActionTrigger') actionTriggered = new EventEmitter<{ action: MenuItem; rowData: any }>();
  @Output('callbackPageChange') eventEmitPageChange = new EventEmitter<any>();
  @Output('callbackSort') eventEmitSort = new EventEmitter<any>();
  @Output('callbackFilter') eventEmitFilter = new EventEmitter<any>();

  selection: any;
  filterVal: string | undefined;
  private userTriggeredSort: boolean = false;

  constructor(private cdr: ChangeDetectorRef) { }

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
    console.log(event);
    this.eventEmitSort.emit({ gridSortMeta: event.multiSortMeta });  
  }

  search(event: any) {
    this.eventEmitFilter.emit(event);
  }

  onKeyDownSearch(event: KeyboardEvent, input: any) {
    if (event.key === "Enter") {
      this.eventEmitFilter.emit(input);
    }
  }

  private onActionTriggered(action: MenuItem, rowData: any) {
    this.actionTriggered.emit({ action, rowData });
  }

}
