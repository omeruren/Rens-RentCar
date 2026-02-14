export interface NavigationModel {
  title: string;
  url?: string;
  icon?: string;
  hasSubNav?: boolean;
  subNavs?: NavigationModel[];
}

export const navigations: NavigationModel[] = [
  {
    title: 'Dashboard',
    url: '/',
    icon: 'bi-speedometer2',
  },
];
