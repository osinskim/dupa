import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { PipesModule } from '../pipes/pipes.module';
import { DirectivesModule } from '../directives/directives.module';
import { Controls } from '.';

@NgModule({
  declarations: [...Controls],
  imports: [
    CommonModule,
    FormsModule,
    PipesModule,
    DirectivesModule
  ],
  exports: [...Controls]
})
export class ControlsModule { }
