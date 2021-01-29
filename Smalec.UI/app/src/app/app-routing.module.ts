import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthGuard, LoginGuard } from './helpers';
import { LoginComponent } from './views/mainpage/login/login.component';
import { MainpageWrapperComponent } from './views/mainpage/mainpage-wrapper/mainpage-wrapper.component';
import { RegisterComponent } from './views/mainpage/register/register.component';


const routes: Routes = [
    {
        path: 'mainpage',
        component: MainpageWrapperComponent,
        canActivate: [LoginGuard],
        children: [
            { path: 'login', component: LoginComponent },
            { path: 'register', component: RegisterComponent }
        ]
    },
    {
        path: 'dashboard',
        canActivate: [AuthGuard],
        loadChildren: () => import('./views/dashboard/dashboard.module').then(m => m.DashboardModule)
    },
    { path: '**', redirectTo: 'dashboard' }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
