import { Component, Input } from "@angular/core";


@Component({
  selector: 'app-icon-component',
  templateUrl: 'icon.component.html'
})

export class IconComponent {
  @Input() width: number | undefined;
  @Input() height: number | undefined;
  @Input() imgSource: string | undefined;
  @Input() imgDesc: string | undefined;

  ngOnInit() {

  }
}
