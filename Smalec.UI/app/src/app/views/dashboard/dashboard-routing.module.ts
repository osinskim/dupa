import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { DashboardComponent } from './dashboard.component';
import { HelpComponent } from './help/help.component';


const routes: Routes = [
  {
    path: '',
    component: DashboardComponent, children: [
      {
        path: 'mainpage',
        loadChildren: () => import('./main-page/main-page.module').then(m => m.MainPageModule)
      },
      {
        path: 'accountsettings',
        loadChildren: () => import('./account-management/account-management.module').then(m => m.AccountManagementModule)
      },
      {
        path: 'help',
        component: HelpComponent,
      },
      { path: '**', redirectTo: 'mainpage' }
    ]
  }

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule { }
