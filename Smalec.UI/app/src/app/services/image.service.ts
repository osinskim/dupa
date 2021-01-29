import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root',
})
export class ImageService {

    getRandomNumbers(): string {
        const typedArray = new Uint8Array(20);
        const randomValues = window.crypto.getRandomValues(typedArray);
        return randomValues.join('');
    }
}
