import { Directive, ElementRef, EventEmitter, HostListener, Input, Output } from '@angular/core';

@Directive({
  selector: '[hideOnOutsideClick]'
})
export class HideOnOutsideClickDirective {

  constructor(private ref: ElementRef<HTMLElement>) { }

  @Input('skippableTargets') skippableTargets: string[];
  @Input('additionalTargets') additionalTargets: string[];
  @Output() clickOutside = new EventEmitter<void>();
  @Output() clickInside = new EventEmitter<void>();

  @HostListener('document:click', ['$event.target'])
  clickOut(target: HTMLElement) {
    const clickedInsideCurrentElement = this.ref.nativeElement.contains(target);
    const clickedOnAdditionalTargets = this.hasTargetBeenClicked(target, this.additionalTargets);
    const clickedOnSkippableTarget = this.hasTargetBeenClicked(target, this.skippableTargets);

    if (clickedInsideCurrentElement || clickedOnAdditionalTargets) {
      this.clickInside.emit();
    }

    if (!clickedInsideCurrentElement
        && !clickedOnSkippableTarget
        && !clickedOnAdditionalTargets) {
      this.clickOutside.emit();
    }
  }

  private hasTargetBeenClicked(clickedTarget: HTMLElement, targets: string[]): boolean {
    if (!targets || targets.length === 0) {
      return false;
    }

    for (let i = 0; i < targets.length; i++) {
      if (clickedTarget.id === targets[i]) {
        return true;
      }
    }

    return false;
  }
}
