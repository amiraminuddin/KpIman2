import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { Column } from "../model/AppModel";

@Component({
  selector: 'app-lookup-component',
  templateUrl: 'lookup.component.html'
})
export class LookupComponent implements OnInit {

  @Input() data: any[] = [];
  @Input() columns: Column[] = [];
  @Input() lookupTable: string = '';
  @Input() selectedData: any;

  @Output('callbackRecordSelected') eventEmitRecordSelected = new EventEmitter<any>();

  ngOnInit(): void {
    console.log(this.selectedData)
  }


  onRowSelect(event: any): void {
    //this.eventEmitRecordSelected.emit(event.data);
  }

  onRowDoubleClick(data: any): void {
    this.eventEmitRecordSelected.emit({ data: data, lookupTable: this.lookupTable });
  }

}
