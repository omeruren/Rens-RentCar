export interface NavigationModel {
  title: string;
  url?: string;
  icon?: string;
  hasSubNav?: boolean;
  subNavs?: NavigationModel[];
  permission: string;
}

export const navigations: NavigationModel[] = [
  {
    title: 'Dashboard',
    url: '/',
    icon: 'bi-speedometer2',
    permission: 'dashboard:view',
  },
  {
    title: 'Branches',
    url: '/branches',
    icon: 'bi-buildings',
    permission: 'branch:view',
  },
  {
    title: 'Roles',
    url: '/roles',
    icon: 'bi-person-lock',
    permission: 'role:view',
  },
];
