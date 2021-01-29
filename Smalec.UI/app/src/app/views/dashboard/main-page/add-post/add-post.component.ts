import { Component, OnInit } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { finalize, takeUntil } from 'rxjs/operators';

import { ApiService, ImageService } from 'src/app/services';
import { DestroyableComponent } from 'src/app/shared';
import { GetNewestUserPost, UserDataState } from 'src/app/store';


@Component({
    selector: 'app-add-post',
    templateUrl: './add-post.component.html',
    styleUrls: ['./add-post.component.scss']
})
export class AddPostComponent extends DestroyableComponent implements OnInit {

    photoSrc: string = '';

    isAddPostModal: boolean = false;
    modalData: { text: string, requestId: string, img: File } = { text: '', requestId: undefined, img: undefined };

    private _isImageLoaded: boolean = false;

    @Select(UserDataState.getProfilePhoto) photo$: Observable<string>;

    constructor(private _store: Store, private _apiService: ApiService,
        private _imageService: ImageService) {
        super();
    }

    ngOnInit(): void {
        this.photo$
            .pipe(takeUntil(this.destroy$))
            .subscribe(
                (result) => {
                    this.photoSrc = result;
                }, (err) => {
                    console.log(err);
                });
    }

    openModal(): void {
        this.isAddPostModal = true;
    }

    closeModal(): void {
        this.isAddPostModal = false;
        this.modalData = { text: '', requestId: undefined, img: undefined };
    }

    addNewPost(): void {
        const formData = new FormData();

        if (this._isImageLoaded) {
            formData.append('file', this.modalData.img, this.modalData.img.name);
        }

        this._apiService.addPost(this.modalData.text, formData)
            .pipe(finalize(() => {
                this._isImageLoaded = false;
            })).subscribe(
                (data) => {
                    this._store.dispatch(new GetNewestUserPost());
                    this.closeModal();
                },
                (err) => {
                    console.error(err);
                }
            );
    }

    onImageLoaded(event: any): void {
        this.modalData.img = event.srcElement.files[0];
        this.modalData.requestId = this._imageService.getRandomNumbers();
        this._isImageLoaded = true;
    }
}
