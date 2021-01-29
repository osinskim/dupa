import { Component, OnInit } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable, Subscription } from 'rxjs';
import { IPost } from 'src/app/models';

import { GetMainPagePostsRequest, PostState } from 'src/app/store';

@Component({
    selector: 'app-main-page',
    templateUrl: './main-page.component.html',
    styleUrls: ['./main-page.component.scss']
})
export class MainPageComponent implements OnInit {

    page: number = 0;
    posts: IPost[] = [];
    postsSub: Subscription;

    @Select(PostState.getPosts) posts$: Observable<IPost[]>;

    constructor(private _store: Store) { }

    ngOnInit(): void {
        this.getMainPagePostsRequest();
        
        this.postsSub = this.posts$.subscribe(
            (data) => {
                if (data.length) {
                    this.posts = [...data];
                }
            }, (err) => {
                console.error(err);
            }
        );
    }

    getMainPagePostsRequest(): void {
        const postLength = this.posts.length;
        const lastDate = postLength ? this.posts[postLength - 1].createdDate : new Date();
        this._store.dispatch(new GetMainPagePostsRequest(this.page, lastDate));
        this.page++; // przenieść do stora
    }

    onScroll(): void {
        if (!this.posts.length) {
            return;
            // it means that getMainPagePostRequest() from ngOnInit() isn't finished yet 
            //or user doesn't have any posts on the mainpage
        }

        this.getMainPagePostsRequest();
    }
}
