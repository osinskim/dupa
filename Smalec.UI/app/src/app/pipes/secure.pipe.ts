import { Pipe, PipeTransform } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Observable} from 'rxjs';
import { map } from 'rxjs/operators';

@Pipe({
  name: 'secure'
})
export class SecurePipe implements PipeTransform {

  constructor(private _http: HttpClient, private _sanitizer: DomSanitizer) { }

  transform(pictureUrl: string): Observable<SafeUrl> {
    // const url = 'https://localhost:42269/FileStorage/GetFile?resource=' + pictureUrl;
    const url = 'http://192.168.0.129:42270/FileStorage/GetFile?resource=' + pictureUrl;

    return this._http
      .get(url, { responseType: 'blob' }).pipe(
        map(value => this._sanitizer.bypassSecurityTrustUrl(URL.createObjectURL(value)))
      );
  }

}
