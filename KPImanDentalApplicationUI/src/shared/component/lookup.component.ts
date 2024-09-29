import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { Column } from "../model/AppModel";

@Component({
  selector: 'app-lookup-component',
  templateUrl: 'lookup.component.html'
})
export class LookupComponent implements OnInit {

  @Input() data: any[] = [];
  @Input() columns: Column[] = [];

  @Output('callbackRecordSelected') eventEmitRecordSelected = new EventEmitter<any>();

  selectedData: any;

  ngOnInit(): void {
  }


  onRowSelect(event: any): void {
    //this.eventEmitRecordSelected.emit(event.data);
  }

  onRowDoubleClick(product: any): void {
    this.eventEmitRecordSelected.emit(product);
  }

}
