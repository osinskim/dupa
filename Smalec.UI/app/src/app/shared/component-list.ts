export interface IComponentData {
    path: string;
    name: string;
    image: string;
    link: string;
    icon: string;
}

export const TITLE_BAR_COMPONENTS: IComponentData[] = [
    { path: '/dashboard/mainpage', name: 'News Feed', image: 'assets/home.png', link: 'mainpage', icon: 'home' },
    { path: '/dashboard/accountsettings', name: 'Ustawienia konta', image: 'assets/account.png', link: 'accountsettings', icon: 'settings' },
    { path: '/dashboard/help', name: 'Pomoc', image: 'assets/question.png', link: 'help', icon: 'help' }
];