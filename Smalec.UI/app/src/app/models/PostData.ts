export enum ReactionType {
    Like = 'like',
    FuckJu = 'fuckJu'
}

export enum ReactedObjectType {
    Post = 'post',
    Comment = 'comment'
}

export interface IReaction {
    amount: number;
    type: ReactionType;
    objectUuid: string;
}

export interface IComment {
    uuid: string;
    createdDate: Date;
    text: string;
    postUuid: string;
    reactions: Array<IReaction>;
    userUuid: string;
}

export interface IPost {
    uuid: string;
    userUuid: string;
    createdDate: Date;
    description: string;
    mediaURL: string;
    reactions: Array<IReaction>;
    comments: Array<IComment>;
}