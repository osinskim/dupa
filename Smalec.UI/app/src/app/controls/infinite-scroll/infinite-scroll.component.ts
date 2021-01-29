import { AfterViewInit, Component, ElementRef, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';

@Component({
    selector: 'app-infinite-scroll',
    templateUrl: './infinite-scroll.component.html',
    styleUrls: ['./infinite-scroll.component.scss']
})
export class InfiniteScrollComponent implements OnInit, OnDestroy, AfterViewInit {

    @Input() options = {};
    @Output() scrolled = new EventEmitter();
    @ViewChild('anchor') anchor: ElementRef<HTMLElement>;

    private _observer: IntersectionObserver;

    constructor(private _host: ElementRef) { }

    ngAfterViewInit(): void {
        this._observer.observe(this.anchor.nativeElement);
    }

    ngOnInit(): void {
        const options = {
            root: this._isHostScrollable() ? this._host.nativeElement : null,
            ...this.options
        };

        this._observer = new IntersectionObserver(([entry]) => {
            if (entry.isIntersecting) {
                this.scrolled.emit();
            }
        }, options);

    }

    ngOnDestroy(): void {
        this._observer.disconnect();
    }

    get element() {
        return this._host.nativeElement;
    }

    private _isHostScrollable() {
        const style = window.getComputedStyle(this.element);
        return style.getPropertyValue('overflow') === 'auto' || style.getPropertyValue('overflow-y') === 'scroll';
    }
}
