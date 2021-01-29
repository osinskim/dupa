import { Injectable } from '@angular/core';
import { State, Selector, Action, StateContext } from '@ngxs/store';
import { tap } from 'rxjs/operators';

import { IPost } from 'src/app/models';
import { ApiService } from 'src/app/services';
import { GetMainPagePostsRequest, GetNewestUserPost } from './post-actions';
import { PostStateModel } from './post-state-model';

@State<PostStateModel>({
    name: 'postState',
    defaults: {
        posts: []
    }
})
@Injectable()
export class PostState {

    constructor(private _apiService: ApiService) { }

    @Selector()
    public static getPosts(state: PostStateModel): Array<IPost> {
        return state.posts;
    }

    @Action(GetMainPagePostsRequest)
    public getPostsRequest({ getState, setState }: StateContext<PostStateModel>, { page, lastDate }: GetMainPagePostsRequest) {
        return this._apiService.getPosts(page, lastDate)
            .pipe(
                tap((result: IPost[]) => {
                    const state = getState();
                    if (result.length) {
                        const posts: PostStateModel = {
                            posts: [...state.posts, ...result]
                        };
                        setState(posts);
                    }
                }))
    }

    @Action(GetNewestUserPost)
    public getNewestUserPost({ getState, setState }: StateContext<PostStateModel>) {
        const state = getState();
        this._apiService.getRecentPost().subscribe( // wywalić subscribe
            (result: IPost) => {
                setState({ posts: [result, ...state.posts] });
            },
            (err) => {
                console.error(err);
            }
        );
    }

    

}
