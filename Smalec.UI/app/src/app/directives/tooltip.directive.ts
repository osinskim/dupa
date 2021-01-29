import { DOCUMENT } from '@angular/common';
import { Directive, ElementRef, HostListener, Inject, Input, Renderer2 } from '@angular/core';

@Directive({
    selector: '[appTooltip]'
})
export class TooltipDirective {

    @Input('tooltipTitle') tooltipTitle: string;
    tooltip: HTMLElement;
    offset = 10;

    constructor(private el: ElementRef, private renderer: Renderer2, @Inject(DOCUMENT) private _document: Document) { }

    @HostListener('mouseenter') onMouseEnter() {
        if (!this.tooltip) { this.show(); }
    }

    @HostListener('mouseleave') onMouseLeave() {
        if (this.tooltip) { this.hide(); }
    }

    show() {
        this.create();
        this.setPosition();
        this.renderer.addClass(this.tooltip, 'tooltip-show');
    }

    hide() {
        this.renderer.removeClass(this.tooltip, 'tooltip-show');
        window.setTimeout(() => {
            this.renderer.removeChild(this._document.body, this.tooltip);
            this.tooltip = null;
        }, 200);
    }

    create() {
        this.tooltip = this.renderer.createElement('span');

        this.renderer.appendChild(
            this.tooltip,
            this.renderer.createText(this.tooltipTitle) // Here is your text
        );

        this.renderer.appendChild(this._document.body, this.tooltip);

        this.renderer.addClass(this.tooltip, 'tooltip');
        this.renderer.addClass(this.tooltip, `tooltip-top`);
        this.renderer.setStyle(this.tooltip, 'transition', `opacity 200ms`);
    }

    setPosition() {
        const hostPos = this.el.nativeElement.getBoundingClientRect();
        const tooltipPos = this.tooltip.getBoundingClientRect();
        const scrollPos = window.pageYOffset || this._document.documentElement.scrollTop || this._document.body.scrollTop || 0;

        const top = hostPos.top - tooltipPos.height - this.offset;
        console.log([this.el.nativeElement]);
        const left = hostPos.left + 5;
        this.renderer.setStyle(this.tooltip, 'top', `${top + scrollPos}px`);
        this.renderer.setStyle(this.tooltip, 'left', `${left}px`);
        console.log(this.tooltip.getBoundingClientRect().left);
    }

}
