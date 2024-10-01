import { Component, Input, OnInit, SimpleChanges } from "@angular/core";
import { Validators } from "../model/AppModel";

@Component({
  selector: 'validation-component',
  templateUrl: 'validation.component.html'
})
export class ValidationComponent implements OnInit {

  @Input() field: string = '';
  @Input() validator: Validators[] = [];


  mandatory: boolean = false;
  error: boolean = false;
  warning: boolean = false;
  message: string | undefined;

  ngOnInit(): void {
    let fieldData = this.validator.find(x => x.field == this.field);
    if (fieldData) {
      this.mandatory = fieldData.validatorsType == 2;
      this.error = fieldData.validatorsType == 0;
      this.warning = fieldData.validatorsType == 1;
      this.message = fieldData.message;
    }
    else {
      this.mandatory = false;
      this.error = false;
      this.warning = false;
      this.message = '';
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['validator']) {
      this.ngOnInit(); // Re-run validation logic when `validators` input changes
    }
  }

}
