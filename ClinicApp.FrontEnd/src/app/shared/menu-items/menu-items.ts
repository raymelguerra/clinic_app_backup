import {Injectable} from '@angular/core';

export interface BadgeItem {
  type: string;
  value: string;
}

export interface ChildrenItems {
  state: string;
  target?: boolean;
  name: string;
  type?: string;
  children?: ChildrenItems[];
}

export interface MainMenuItems {
  state: string;
  short_label?: string;
  main_state?: string;
  target?: boolean;
  name: string;
  type: string;
  icon: string;
  badge?: BadgeItem[];
  children?: ChildrenItems[];
}

export interface Menu {
  label: string;
  main: MainMenuItems[];
}

const MENUITEMS = [
  {
    label: 'Navigation',
    main: [
      {
        state: 'dashboard',
        short_label: 'D',
        name: 'Dashboard',
        type: 'link',
        icon: 'ti-dashboard'
      },
      /*{
        state: 'basic',
        short_label: 'B',
        name: 'Basic Components',
        type: 'sub',
        icon: 'ti-layout-grid2-alt',
        children: [
          {
            state: 'button',
            name: 'Button'
          },
          {
            state: 'typography',
            name: 'Typography'
          },
          {
            state: 'accordion',
            name: 'Accordion'
          }
        ]
      },
      {
        state: 'notifications',
        short_label: 'n',
        name: 'Notifications',
        type: 'link',
        icon: 'ti-crown'
      },*/
    ],
  },
  {
    label: 'Generals',
    main: [
      {
        state: 'human_resource',
        short_label: 'B',
        name: 'Human resources',
        type: 'sub',
        icon: 'icofont icofont-users-social st-icon',
        children: [
          {
            state: 'contractor',
            name: 'Contractor'
          },
          {
            state: 'client',
            name: 'Client'
          }
        ]
      },
      {
        state: 'building_resources',
        short_label: 'B',
        name: 'Building and Resources',
        type: 'sub',
        icon: 'ti-home',
        children: [
          {
            state: 'company',
            name: 'Company'
          }
        ]
      },
      // {
      //   state: 'authentication',
      //   short_label: 'B',
      //   name: 'Authorization',
      //   type: 'sub',
      //   icon: 'icofont icofont-users-social st-icon',
      //   children: [
      //     {
      //       state: 'register',
      //       name: 'Register users'
      //     },
      //     {
      //       state: 'login',
      //       name: 'For test login'
      //     }
      //   ]
      // },
    ]
  },
  {
    label: 'Accounting and finances',
    main: [
      {
        state: 'billing',
        short_label: 'B',
        name: 'Billing',
        type: 'sub',
        icon: 'ti-stats-up',
        children: [
          {
            state: 'period-payment',
            name: 'Period and payment'
          },
          {
            state: 'service-log',
            name: 'Service log'
          },
          {
            state: 'pending',
            name: 'Pending'
          }
        ]
      }
    ]
  },
 /* {
    label: 'Tables',
    main: [
      {
        state: 'bootstrap-table',
        short_label: 'B',
        name: 'Bootstrap Table',
        type: 'link',
        icon: 'ti-receipt'
      }
    ]
  },
  {
    label: 'Map And Extra Pages ',
    main: [
      {
        state: 'map',
        short_label: 'M',
        name: 'Maps',
        type: 'link',
        icon: 'ti-map-alt'
      },
      {
        state: 'authentication',
        short_label: 'A',
        name: 'Authentication',
        type: 'sub',
        icon: 'ti-id-badge',
        children: [
          {
            state: 'login',
            type: 'link',
            name: 'Login',
            target: true
          }, {
            state: 'registration',
            type: 'link',
            name: 'Registration',
            target: true
          }
        ]
      },
      {
        state: 'user',
        short_label: 'U',
        name: 'User Profile',
        type: 'link',
        icon: 'ti-user'
      }
    ]
  },
  {
    label: 'Other',
    main: [
      {
        state: '',
        short_label: 'M',
        name: 'Menu Levels',
        type: 'sub',
        icon: 'ti-direction-alt',
        children: [
          {
            state: '',
            name: 'Menu Level 2.1',
            target: true
          }, {
            state: '',
            name: 'Menu Level 2.2',
            type: 'sub',
            children: [
              {
                state: '',
                name: 'Menu Level 2.2.1',
                target: true
              },
              {
                state: '',
                name: 'Menu Level 2.2.2',
                target: true
              }
            ]
          }, {
            state: '',
            name: 'Menu Level 2.3',
            target: true
          }, {
            state: '',
            name: 'Menu Level 2.4',
            type: 'sub',
            children: [
              {
                state: '',
                name: 'Menu Level 2.4.1',
                target: true
              },
              {
                state: '',
                name: 'Menu Level 2.4.2',
                target: true
              }
            ]
          }
        ]
      },
      {
        state: 'simple-page',
        short_label: 'S',
        name: 'Simple Page',
        type: 'link',
        icon: 'ti-layout-sidebar-left'
      }
    ]
  }*/
];

@Injectable()
export class MenuItems {
  getAll(): Menu[] {
    return MENUITEMS;
  }

  /*add(menu: Menu) {
    MENUITEMS.push(menu);
  }*/
}
