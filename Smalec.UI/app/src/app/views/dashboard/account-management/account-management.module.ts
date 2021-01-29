import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountManagementRoutingModule } from './account-management-routing.module';
import { AccountSettingsComponent } from './account-settings.component';
import { ImageCropperModule } from 'ngx-image-cropper';

import { ImageManagementComponent } from './image-management/image-management.component';
import { NameChangeComponent } from './name-change/name-change.component';
import { ControlsModule } from 'src/app/controls/controls.module';
import { PipesModule } from 'src/app/pipes/pipes.module';


@NgModule({
  declarations: [
    AccountSettingsComponent,
    ImageManagementComponent,
    NameChangeComponent
  ],
  imports: [
    CommonModule,
    AccountManagementRoutingModule,
    ControlsModule,
    ImageCropperModule,
    PipesModule
  ]
})
export class AccountManagementModule { }
