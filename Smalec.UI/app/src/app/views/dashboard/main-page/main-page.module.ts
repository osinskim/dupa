import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MainPageRoutingModule } from './main-page-routing.module';
import { MainPageComponent } from './main-page.component';
import { ControlsModule } from 'src/app/controls/controls.module';
import { AddPostComponent } from './add-post/add-post.component';
import { PipesModule } from 'src/app/pipes/pipes.module';


@NgModule({
  declarations: [
    MainPageComponent,
    AddPostComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    MainPageRoutingModule,
    ControlsModule,
    PipesModule
  ]
})
export class MainPageModule { }
