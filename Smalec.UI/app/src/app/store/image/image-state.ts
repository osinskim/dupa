import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext } from '@ngxs/store';
import { EMPTY } from 'rxjs';
import { catchError, shareReplay } from 'rxjs/operators';

import { AddNewImage } from './image-actions';
import { ImageStateModel } from './image-state-model';

@State<ImageStateModel[]>({
    name: 'imageState',
    defaults: []
})
@Injectable()
export class ImageState {

    private _cache: any = {};

    constructor(private http: HttpClient) { }

    @Selector()
    public static getImages(state: ImageStateModel[]): ImageStateModel[] {
        return state;
    }

    @Action(AddNewImage)
    public addNewImage({ getState, setState }: StateContext<ImageStateModel[]>, { imgSrc }: AddNewImage): void {
        const state = getState();

        if (this._cache[imgSrc]) {
            this._cache[imgSrc].subscribe(
                (result) => {
                    setState([...state, result]);
                }
            );
        } else {
            this._cache[imgSrc] = this.http.get('https://localhost:42269/FileStorage/GetFile?resource=' + imgSrc).pipe(
                shareReplay(1),
                catchError(err => {
                    delete this._cache[imgSrc];
                    return EMPTY;
                })
            );
        }
    }
}