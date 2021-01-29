import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';

import { IPost, ReactionType, ReactedObjectType, User } from '../models';

@Injectable({
    providedIn: 'root'
})
export class ApiService {

    private _apiBase = 'http://localhost:42270/';
    // private _apiBase = 'https://localhost:42269/';
    private _auth = 'Auth/';
    private _userManagement = 'UserManagement/';
    private _fileStorage = 'FileStorage/';
    private _posts = 'Posts/';
    private _socialFacade = 'SocialFacade/';
    private _social = 'Social/';

    constructor(private _http: HttpClient) { }

    public signIn(login: string, password: string): Observable<HttpResponse<any>> {
        return this._http.post(this._apiBase + this._auth + 'SignIn', { UserName: login, Password: password }, { withCredentials: true, observe: 'response'});
    }

    public refreshToken(): Observable<any> {
        return this._http.post(this._apiBase + this._auth + 'RefreshToken', {}, { withCredentials: true });
    }

    public signOut(): Observable<any> {
        return this._http.get(this._apiBase + this._auth + 'SignOut', { withCredentials: true });
    }

    public getCurrentUserData(): Observable<User> {
        return this._http.get<User>(this._apiBase + this._userManagement + 'GetCurrentUserData');
    }

    public getUserData(uuid: string): Observable<User> {
        return this._http.get<User>(this._apiBase + this._userManagement + 'GetUserData' + '?userUuid=' + uuid)
    }

    public updateCurrentUserData(userData: User): Observable<any> {
        return this._http.put(this._apiBase + this._userManagement + 'UpdateCurrentUserData', userData);
    }

    public register(email: string, password: string, name: string): Observable<string[]> {
        return this._http.post<string[]>(this._apiBase + this._userManagement + 'AddNewUser', { UserName: email, Password: password, Name: name });
    }

    public savePhotoWithProgress(formData: FormData, requestId: string): Observable<any> {
        formData.append('requestId', requestId);
        return this._http.post(this._apiBase + this._fileStorage + 'SavePhoto', formData, { reportProgress: true, observe: 'events' })
    }

    public updateProfilePhoto(formData: FormData): Observable<any> {
        return this._http.post(this._apiBase + this._socialFacade + this._social + 'ChangeProfilePhoto', formData, { reportProgress: true, observe: 'events' })
    }

    public savePhoto(formData: FormData, requestId: string): Observable<any> {
        formData.append('requestId', requestId);
        return this._http.post(this._apiBase + this._fileStorage + 'SavePhoto', formData);
    }

    public addPost(description: string, formData: FormData): Observable<any> {
        formData.append('description', description);
        return this._http.post(this._apiBase + this._socialFacade + this._social + 'AddPost', formData);
    }

    public addFriend(profileUuid: string): Observable<any> {
        return this._http.post<any>(this._apiBase + 'Friendship/Add', { UserToAdd: profileUuid });
    }

    public getPosts(page: number, date: Date): Observable<IPost[]> {
        const url = this._apiBase + this._socialFacade + this._social + `GetMainpagePosts?page=${page}&lastPostFetched=${encodeURIComponent(date.toLocaleString())}`;
        return this._http.get<IPost[]>(url);
    }

    public getRecentPost(): Observable<IPost> {
        const url = this._apiBase + this._posts + 'GetMyRecentPost';
        return this._http.get<IPost>(url);
    }

    public addComment(postOwnerUuid: string, text: string, postUuid: string, ): Observable<any> {
        const url = this._apiBase + this._socialFacade + this._social + 'AddComment';
        return this._http.post(url, {postOwnerUuid, text, postUuid});
    }

    public addReaction(reaction: ReactionType, objectId: string, reactedObjectType: ReactedObjectType): Observable<any> {
        const url = this._apiBase + this._social + this._posts + 'AddReaction';
        return this._http.post(url, {reaction, objectId, reactedObjectType});
    }

    public search(keywords: string): Observable<User[]> {
        const url = this._apiBase + this._userManagement + "Search?keywords=" + encodeURIComponent(keywords);
        return this._http.get<User[]>(url);
    }
}
