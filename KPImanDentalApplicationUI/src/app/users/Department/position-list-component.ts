import { ChangeDetectorRef, Component, Input } from "@angular/core";
import { PositionDto } from "../../../shared/model/AppModel";

@Component({
  selector: 'app-position-list-component',
  templateUrl: 'position-list-component.html',
  styleUrls: ['../user-list-component.css']
})
export class positionComponent {
  @Input() position: any[] = [];

  //positionData: PositionDto[] = [];

  constructor(private cdr: ChangeDetectorRef) { }

  ngOnInit() {
    this.position;
    this.cdr.detectChanges();
  }

  getData() {

  }
}
