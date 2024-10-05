import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";

@Component({
  selector: 'text-area-component',
  templateUrl: 'text.area.component.html'
})
export class TextAreaComponent implements OnInit {

  @Input() input: string = '';
  @Input() field: string = '';

  @Output("callbackOutput") output = new EventEmitter<any>();

  ngOnInit(): void {
    
  }

  closeModal(data: any) {
    this.output.emit({ data: data, textField: this.field })
  }

}
