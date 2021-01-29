import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

import { slideInOut } from 'src/app/animations';
import { IPost, IReaction, ReactedObjectType, ReactionType, User } from 'src/app/models';
import { ApiService } from 'src/app/services';
import { DestroyableComponent } from 'src/app/shared';
import { AddNewUser, UsersState } from 'src/app/store';
import { AuthorSize } from '../author/author.component';

@Component({
    selector: 'app-post-view',
    templateUrl: './post-view.component.html',
    styleUrls: ['./post-view.component.scss'],
    animations: [slideInOut]
})
export class PostViewComponent extends DestroyableComponent implements OnInit, OnDestroy {

    _post: IPost;
    @Input() set post(post: IPost) {
        this._post = post;
        post?.comments?.forEach(x => {
            this.loadUserData(x.userUuid);
        })
    }

    @Select(UsersState.getUsers) users$: Observable<User[]>;

    reactionType = ReactionType;
    reactedObjectType = ReactedObjectType;
    AuthorSize: typeof AuthorSize = AuthorSize;

    postOwner: User;
    usersData: User[];

    commentText: string;
    likesAmount: number = 0;
    fuckJuAmount: number = 0;
    commentsVisible: boolean = false;

    constructor(private _store: Store, private _apiService: ApiService) {
        super();
    }

    ngOnInit(): void {
        this.users$.pipe(takeUntil(this.destroy$))
            .subscribe(
                (data) => {
                    this.usersData = data;
                    if (!this.postOwner) {
                        this.postOwner = this.findUserData(this._post.userUuid);
                    }
                }, (err) => {
                    console.log(err);
                }
            );

        this.likesAmount = this.getReactionAmount(this._post.reactions, ReactionType.Like);
        this.fuckJuAmount = this.getReactionAmount(this._post.reactions, ReactionType.FuckJu);
    }

    getReactionAmount(arr: IReaction[], reaction: ReactionType): number {
        if (!arr) {
            return 0;
        }
        const reactions = arr.filter(reac => reac.type === reaction);
        return reactions.length;
    }

    addComment(): void {
        this._apiService.addComment(this.postOwner.uuid, this.commentText, this._post.uuid).subscribe((data) => {
            // dispaczuj jakom akcje ze stora którego nie ma jeszce
        }, (err) => {
            console.error(err);
        });
    }

    addReaction(reaction: ReactionType, objectId: string, objectType: ReactedObjectType) {
        this._apiService.addReaction(reaction, objectId, objectType).subscribe((data) => {
            // dispaczuj jakom akcje ze stora którego nie ma jeszce
            console.log('dupa', data);
        }, (err) => {
            console.error(err);
        });
    }

    findUserData(userUuid: string): User {
        const user = this.usersData.find(u => u.uuid === userUuid);

        if (!user) {
            this._store.dispatch(new AddNewUser(userUuid));
        }

        return user;
    }

    private loadUserData(userUuid: string) {
        this._store.dispatch(new AddNewUser(userUuid));
    }
}
