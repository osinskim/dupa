import { Component, OnDestroy } from "@angular/core";
import { ReplaySubject } from "rxjs";

@Component({template: ''})
export class DestroyableComponent implements OnDestroy {
    public destroy$: ReplaySubject<boolean> = new ReplaySubject(1);

    ngOnDestroy() {
        this.destroy$.next(true);
        this.destroy$.complete();
    }
}