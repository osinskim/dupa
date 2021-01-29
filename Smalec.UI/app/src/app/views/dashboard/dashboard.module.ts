import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { LeftMenuComponent } from './left-menu/left-menu.component';
import { TopBarComponent } from './top-bar/top-bar.component';
import { DashboardComponent } from './dashboard.component';
import { ControlsModule } from 'src/app/controls/controls.module';
import { DashboardRoutingModule } from './dashboard-routing.module';
import { HelpComponent } from './help/help.component';
import { PipesModule } from 'src/app/pipes/pipes.module';
import { SearchComponent } from './search/search.component';
import { DirectivesModule } from 'src/app/directives/directives.module';

@NgModule({
  declarations: [
    DashboardComponent,
    LeftMenuComponent,
    TopBarComponent,
    HelpComponent,
    SearchComponent
  ],
  imports: [
    CommonModule,
    ControlsModule,
    DashboardRoutingModule,
    PipesModule,
    FormsModule,
    DirectivesModule
  ]
})
export class DashboardModule { }
