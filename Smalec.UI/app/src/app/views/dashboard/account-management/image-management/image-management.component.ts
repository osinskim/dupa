import { Component, OnInit } from '@angular/core';
import { HttpEventType } from '@angular/common/http';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { ImageCroppedEvent } from 'ngx-image-cropper';

import { LoadUserData, UserDataState } from 'src/app/store';
import { ApiService, ImageService } from 'src/app/services';


@Component({
    selector: 'app-image-management',
    templateUrl: './image-management.component.html',
    styleUrls: ['./image-management.component.scss']
})
export class ImageManagementComponent implements OnInit {

    photoSrc: string;

    @Select(UserDataState.getProfilePhoto) profilePhoto$: Observable<string>;

    isNewPhotoUploadModal: boolean = false;
    imageChangedEvent: any = '';
    croppedImage: any = '';
    progress: number = 0;

    constructor(private _apiService: ApiService, private _store: Store, private _imageService: ImageService) { }

    ngOnInit(): void {
        this.profilePhoto$.subscribe((result) => {
            this.photoSrc = result;
        });
    }

    fileChangeEvent(event: any): void {
        this.imageChangedEvent = event;
        this.openModal();
    }

    imageCropped(event: ImageCroppedEvent) {
        this.croppedImage = event.base64;
    }

    openModal(): void {
        // TODO: zrobić serwis do modalu
        this.isNewPhotoUploadModal = true;
        // document.body.style.overflow = 'hidden';
    }

    closeModal(): void {
        this.isNewPhotoUploadModal = false;
        // document.body.style.overflow = 'auto';
    }

    saveProfilePhoto(): void {
        this.croppedImage = this.croppedImage.replace('data:image/png;base64,', '');
        const fileName = this.imageChangedEvent.srcElement.files[0].name;
        const blobObject = this._convertb64ToBlob(this.croppedImage);
        const fileToUpload = this._convertBlobToFile(blobObject, fileName);

        const formData = new FormData();
        formData.append('file', fileToUpload, fileToUpload.name);

        // const requestId = this._imageService.getRandomNumbers();
        // combineLatest([
        //     this._apiService.savePhotoWithProgress(formData, requestId),
        //     this._apiService.setProfilePhoto(requestId)
        // ])
        //     .pipe(
        //         finalize(() => {
        //             this.closeModal();
        //         }))
        //     .subscribe(
        //         ([event, anything]) => {
        //             if (event.type === HttpEventType.UploadProgress) {
        //                 this.progress = Math.round(100 * event.loaded / event.total);
        //             } else if (event.type === HttpEventType.Response) {
        //                 this._clearProgress();
        //                 this._store.dispatch(new LoadUserData());
        //             }
        //         }, (error) => {
        //             console.error("chujozzza...");
        //             console.error(error);
        //         }
        //     );
        this._apiService.updateProfilePhoto(formData)
            .pipe(
                finalize(() => {
                    this.closeModal();
                }))
            .subscribe(
                (event) => {
                    if (event.type === HttpEventType.UploadProgress) {
                        this.progress = Math.round(100 * event.loaded / event.total);
                    } else if (event.type === HttpEventType.Response) {
                        this._clearProgress();
                        this._store.dispatch(new LoadUserData());
                    }
                }, (error) => {
                    console.error("chujozzza...");
                    console.error(error);
                }
            );
    }

    private _clearProgress(): void {
        this.progress = 0;
    }

    private _convertb64ToBlob(b64Data, contentType = '', sliceSize = 512): Blob {
        const byteCharacters = atob(b64Data);
        const byteArrays = [];

        for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
            const slice = byteCharacters.slice(offset, offset + sliceSize);

            const byteNumbers = new Array(slice.length);
            for (let i = 0; i < slice.length; i++) {
                byteNumbers[i] = slice.charCodeAt(i);
            }

            const byteArray = new Uint8Array(byteNumbers);
            byteArrays.push(byteArray);
        }

        const blob = new Blob(byteArrays, { type: contentType });
        return blob;
    }

    private _convertBlobToFile(blobObject: Blob, fileName: string): File {
        const b: any = blobObject;
        b.lastModifiedDate = new Date();
        b.name = fileName;

        return b as File;
    }

}
