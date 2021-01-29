import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TooltipDirective } from './tooltip.directive';
import { HideOnOutsideClickDirective } from './hide-on-outside-click.directive';

@NgModule({
  declarations: [
    TooltipDirective,
    HideOnOutsideClickDirective
  ],
  imports: [
    CommonModule
  ],
  exports: [
    TooltipDirective,
    HideOnOutsideClickDirective
  ]
})
export class DirectivesModule { }
