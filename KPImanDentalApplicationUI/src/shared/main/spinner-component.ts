import { Component, Input } from "@angular/core";

@Component({
  selector: 'spinner-component',
  templateUrl: 'spinner-component.html',
  styleUrls: ["spinner-component.css"]
})
export class SpinnerComponent {
  @Input() isSpinnerShow!: boolean;


  ngOnInit(): void {
    this.isSpinnerShow;
  }
}
