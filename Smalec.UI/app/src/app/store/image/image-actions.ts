export class AddNewImage {
    static readonly type = '[ImageState] add image';
    constructor(public imgSrc: string) { }
}