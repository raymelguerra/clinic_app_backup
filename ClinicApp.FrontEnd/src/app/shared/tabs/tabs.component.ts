
import { AfterContentInit, Component, ContentChildren, Input, QueryList } from '@angular/core';
import { TabComponent  } from './tab.component';

@Component({
  selector: 'tabs',
  template: `
    
    <ul class="tabs">
      <li *ngFor="let tab of tabs" (click)="activateTab(tab)" [class.active]="tab.active">
        <a>{{tab.name}}</a>
      </li>
    </ul>
    <ng-content></ng-content>
  `,
  styles: [`
    ul {
      margin: 0;
      padding: 0;
      white-space:nowrap;
      margin-bottom: -1px;
      height: 2.25em;
    }

    li {
      float: left;
      list-style: none;
      margin: 0;
      padding: .25em .25em 0;
      height: 2em;
      overflow: hidden;
      position: relative;
      z-index: 1;
      border-bottom: 1px solid #FFF;
    }

    li.active {
      z-index: 3;
    }

    a {
      float: left;
      height: 2em;
      line-height: 2em;
      border-radius: 0;
      background: transparent;
      border: 1px solid white;
      border-bottom: 0;
      padding: 0 10px;
      color: #000;
      text-decoration: none;
    }

    .active a {
      background: #FFF;
      box-shadow: #CCC 0 0 .25em;
      border: 1px solid #CCC;
    }

    ::ng-deep .pane {
      position: relative;
      z-index: 2;
      padding: 2em 1em;
      margin: 0 0.25em;
      border: 1px solid #CCC;
      background: #FFF;
      border-radius: 0;
      box-shadow: #CCC 0 0 .25em;
    }
`]
})
export class TabsComponent implements AfterContentInit {
  @ContentChildren(TabComponent) tabs: QueryList<TabComponent>;

  ngAfterContentInit() {
    let activeTabs = this.tabs.filter(tab => tab.active);
    if(!activeTabs.length) {
      this.activateTab(this.tabs.first);
    }
  }
  
  activateTab(tab: TabComponent){
    this.tabs.toArray().forEach(item => item.active = false);
    tab.active = true;
  }
}
