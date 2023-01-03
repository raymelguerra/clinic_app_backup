import { AfterContentInit, Component, ContentChildren, Input, QueryList, ViewEncapsulation} from '@angular/core';

@Component({
  selector: 'tab',
  styles: [`
    
  `],
  template: `
    <div [hidden]="!active" class="pane">
      <div class="content">
        <ng-content></ng-content>
      </div>
    </div>
  `,
})
export class TabComponent {
  @Input() name: string;
  @Input() active = false;
}