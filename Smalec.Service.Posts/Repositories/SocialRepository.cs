using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Smalec.Lib.Social.Dtos;
using Smalec.Lib.Social.Enums;
using Smalec.Service.Posts.Abstraction;
using Smalec.Service.Posts.Helpers;
using Smalec.Service.Posts.Models;
using Smalec.Lib.Shared.Helpers;

namespace Smalec.Service.Posts.Repositories
{
    public class SocialRepository : ISocialRepository
    {
        private const int POSTS_PER_PAGE = 10;
        private readonly string _connectionString;

        public SocialRepository(AppSettingsBase appSettings)
        {
            _connectionString = appSettings.ConnectionString;
        }

        public async Task AddComment(Comment comment)
        {
            using (var db = new SqlConnection(_connectionString))
                await db.QueryAsync(
                $@"INSERT INTO [Comments]
                    (
                        Uuid,
                        UserUuid,
                        Text,
                        CreatedDate,
                        PostId,
                        PostUuid
                    )
                    VALUES
                    (
                        @{nameof(comment.Uuid)},
                        @{nameof(comment.UserUuid)},
                        @{nameof(comment.Text)},
                        @{nameof(comment.CreatedDate)},
                        (SELECT TOP 1 Id FROM [Posts] WHERE Uuid = @{nameof(comment.PostUuid)}),
                        @{nameof(comment.PostUuid)}
                    )",
                    new
                    {
                        comment.Uuid,
                        comment.UserUuid,
                        comment.Text,
                        comment.PostUuid,
                        comment.CreatedDate
                    }
                );
        }

        public async Task AddOrUpdateReaction(Reaction reaction)
        {
            string columnToUpdate = string.Empty;

            if(reaction.PostId > 0 && reaction.CommentId == -1)
                columnToUpdate = "PostId";

            if(reaction.CommentId > 0 && reaction.PostId == -1)
                columnToUpdate = "CommentId";

            if(columnToUpdate == string.Empty)
                throw new Exception("coś sie spierdoliło przy AddOrUpdateReaction()");

            using (var db = new SqlConnection(_connectionString))
                await db.QueryAsync(
                @$"UPDATE [Reactions]
                    SET
                        Type = @{nameof(reaction.Type)},
                        CreatedDate = @{nameof(reaction.CreatedDate)}
                    WHERE
                        {columnToUpdate} = @{columnToUpdate}
                        AND UserUuid = @{nameof(reaction.UserUuid)}

                    IF @@ROWCOUNT = 0
                    INSERT INTO [Reactions] 
                    (
                        UserUuid,
                        Type,
                        CreatedDate,
                        {columnToUpdate}
                    )
                    VALUES
                    (
                        @{nameof(reaction.UserUuid)},
                        @{nameof(reaction.Type)},
                        @{nameof(reaction.CreatedDate)},
                        @{columnToUpdate}
                    )",
                    new
                    {
                        reaction.UserUuid,
                        PostId = reaction.PostId,
                        CommentId = reaction.CommentId,
                        reaction.CreatedDate,
                        reaction.Type
                    }
                );
        }

        public async Task AddPost(Post post)
        {
            using (var db = new SqlConnection(_connectionString))
                await db.QueryAsync(
                $@"INSERT INTO [Posts]
                    (
                        Uuid,
                        Description,
                        UserUuid,
                        CreatedDate,
                        MediaURL
                    ) 
                    VALUES
                    (
                        @{nameof(post.Uuid)},
                        @{nameof(post.Description)},
                        @{nameof(post.UserUuid)},
                        @{nameof(post.CreatedDate)},
                        @{nameof(post.MediaURL)}
                    )",
                    new
                    {
                        post.Uuid,
                        post.Description,
                        post.UserUuid,
                        post.CreatedDate,
                        post.MediaURL
                    }
                );
        }

        public async Task<IEnumerable<Comment>> GetComments(IEnumerable<string> postsUuids)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var result = await db.QueryAsync<Comment>(
                $"SELECT * FROM [Comments] WHERE PostUuid IN @{nameof(postsUuids)}",
                new { postsUuids });

                return result;
            }
        }

        public async Task<RawPostData> GetPostsByUserUuid(string userUuid, int page, string lastPostFetched)
        {
            var offset = (page - 1) * POSTS_PER_PAGE;
            using (var db = new SqlConnection(_connectionString))
            {
                using(var multiResult = await db.QueryMultipleAsync(
                $@" DECLARE @PostsIds TABLE (Id int, Uuid varchar(40));
                    DECLARE @CommentsIds TABLE (Id int, Uuid varchar(40));

                    INSERT INTO @PostsIds(Id, Uuid)
                        SELECT Id, Uuid FROM [Posts] 
                        WHERE CreatedDate < @{nameof(lastPostFetched)} AND UserUuid = @{nameof(userUuid)}
                        ORDER BY CreatedDate DESC
                        OFFSET @{nameof(offset)} ROWS FETCH NEXT @{nameof(POSTS_PER_PAGE)} ROWS ONLY;
                    
                    SELECT p.UserUuid, p.Id, p.Uuid, p.CreatedDate, p.Description, p.MediaURL
                    FROM Posts p
                    INNER JOIN @PostsIds ids ON ids.Id=p.Id;
                        
                    SELECT COUNT(1) as Amount, r.Type, ids.Uuid as ObjectUuid
                    FROM Reactions r
                    INNER JOIN @PostsIds ids ON ids.Id=r.PostId
                    GROUP BY r.Type, ids.Uuid

                    INSERT INTO @CommentsIds(Id, Uuid)
                        SELECT c.Id, c.Uuid
                        FROM Comments c
                        INNER JOIN @PostsIds p ON p.Id=c.PostId
                        
                    SELECT c.Id, c.CreatedDate, c.Text, c.PostUuid as PostUuid, c.UserUuid as UserUuid
                    FROM Comments c
                    INNER JOIN @CommentsIds ids ON ids.Id=c.Id

                    SELECT COUNT(1) as Amount, r.Type, ids.Uuid as ObjectUuid
                    FROM Reactions r
                    INNER JOIN @CommentsIds ids ON ids.Id=r.CommentId
                    GROUP BY r.Type, ids.Uuid
                    ",
                new {
                    page,
                    userUuid,
                    offset,
                    lastPostFetched = DateTime.Parse(lastPostFetched, new CultureInfo("pl-PL")).ToString("yyyy-MM-dd HH:mm:ss"),
                    POSTS_PER_PAGE
                }))
                {
                    return new RawPostData
                    {
                        Posts = await multiResult.ReadAsync<PostDto>(),
                        PostsReactions = await multiResult.ReadAsync<ReactionDto>(),
                        Comments = await multiResult.ReadAsync<CommentDto>(),
                        CommentsReactions = await multiResult.ReadAsync<ReactionDto>()
                    };
                }
            }
        }

        public async Task<RawPostData> GetMyLastPost(string userUuid)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                using(var multiResult = await db.QueryMultipleAsync(
                $@" DECLARE @PostsIds TABLE (Id int, Uuid varchar(40));
                    DECLARE @CommentsIds TABLE (Id int, Uuid varchar(40));

                    INSERT INTO @PostsIds(Id, Uuid)
                        SELECT TOP 1 Id, Uuid FROM [Posts] 
                        WHERE UserUuid = @{nameof(userUuid)}
                        ORDER BY CreatedDate DESC;
                    
                    SELECT p.UserUuid, p.Id, p.Uuid, p.CreatedDate, p.Description, p.MediaURL
                    FROM Posts p
                    INNER JOIN @PostsIds ids ON ids.Id=p.Id;
                        
                    SELECT COUNT(1) as Amount, r.Type, ids.Uuid as ObjectUuid
                    FROM Reactions r
                    INNER JOIN @PostsIds ids ON ids.Id=r.PostId
                    GROUP BY r.Type, ids.Uuid

                    INSERT INTO @CommentsIds(Id, Uuid)
                        SELECT c.Id, c.Uuid
                        FROM Comments c
                        INNER JOIN @PostsIds p ON p.Id=c.PostId
                        
                    SELECT c.Id, c.CreatedDate, c.Text, c.PostUuid as PostUuid, c.UserUuid as UserUuid
                    FROM Comments c
                    INNER JOIN @CommentsIds ids ON ids.Id=c.Id

                    SELECT COUNT(1) as Amount, r.Type, ids.Uuid as ObjectUuid
                    FROM Reactions r
                    INNER JOIN @CommentsIds ids ON ids.Id=r.CommentId
                    GROUP BY r.Type, ids.Uuid
                    ",
                new { userUuid }))
                {
                    return new RawPostData
                    {
                        Posts = await multiResult.ReadAsync<PostDto>(),
                        PostsReactions = await multiResult.ReadAsync<ReactionDto>(),
                        Comments = await multiResult.ReadAsync<CommentDto>(),
                        CommentsReactions = await multiResult.ReadAsync<ReactionDto>()
                    };
                }
            }
        }

        public async Task<IEnumerable<Reaction>> GetReactions(IEnumerable<string> objectsUuids, ReactedObjectType objectType)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                if (objectType == ReactedObjectType.Comment)
                {
                    return await db.QueryAsync<Reaction>(
                            $"SELECT * FROM [Reactions] WHERE CommentUuid IN @{nameof(objectsUuids)}",
                            new { objectsUuids });
                }

                if (objectType == ReactedObjectType.Post)
                {
                    return await db.QueryAsync<Reaction>(
                            $"SELECT * FROM [Reactions] WHERE PostUuid IN @{nameof(objectsUuids)}",
                            new { objectsUuids });
                }
            }
            return Enumerable.Empty<Reaction>();
        }

        public async Task<RawPostData> GetMainpagePosts(int page, string lastPostFetched, IEnumerable<string> usersUuids)
        {
            var offset = (page - 1) * POSTS_PER_PAGE;
            using (var db = new SqlConnection(_connectionString))
            {
                using(var multiResult = await db.QueryMultipleAsync(
                $@" DECLARE @PostsIds TABLE (Id int, Uuid varchar(40), Date Datetime2);
                    DECLARE @CommentsIds TABLE (Id int, Uuid varchar(40));

                    INSERT INTO @PostsIds(Id, Uuid, Date)
                        SELECT DISTINCT p.Id, p.Uuid, p.CreatedDate
                        FROM [Posts] p 
                        WHERE p.CreatedDate < @{nameof(lastPostFetched)} AND p.UserUuid IN @{nameof(usersUuids)}
                        ORDER BY p.CreatedDate DESC
                        OFFSET @{nameof(offset)} ROWS FETCH NEXT @{nameof(POSTS_PER_PAGE)} ROWS ONLY;
                    
                    SELECT p.UserUuid, p.Id, p.Uuid, p.CreatedDate, p.Description, p.MediaURL
                    FROM Posts p
                    INNER JOIN @PostsIds ids ON ids.Id=p.Id;
                        
                    SELECT COUNT(1) as Amount, r.Type, ids.Uuid as ObjectUuid
                    FROM Reactions r
                    INNER JOIN @PostsIds ids ON ids.Id=r.PostId
                    GROUP BY r.Type, ids.Uuid

                    INSERT INTO @CommentsIds(Id, Uuid)
                        SELECT c.Id, c.Uuid
                        FROM Comments c
                        INNER JOIN @PostsIds p ON p.Id=c.PostId
                        
                    SELECT c.Id, c.CreatedDate, c.Text, c.PostUuid as PostUuid, c.UserUuid as UserUuid
                    FROM Comments c
                    INNER JOIN @CommentsIds ids ON ids.Id=c.Id

                    SELECT COUNT(1) as Amount, r.Type, ids.Uuid as ObjectUuid
                    FROM Reactions r
                    INNER JOIN @CommentsIds ids ON ids.Id=r.CommentId
                    GROUP BY r.Type, ids.Uuid
                    ",
                new {
                        page,
                        offset,
                        lastPostFetched = DateTime.Parse(lastPostFetched, new CultureInfo("pl-PL")).ToString("yyyy-MM-dd HH:mm:ss"),
                        POSTS_PER_PAGE,
                        usersUuids
                }))
                {
                    return new RawPostData
                    {
                        Posts = await multiResult.ReadAsync<PostDto>(),
                        PostsReactions = await multiResult.ReadAsync<ReactionDto>(),
                        Comments = await multiResult.ReadAsync<CommentDto>(),
                        CommentsReactions = await multiResult.ReadAsync<ReactionDto>()
                    };
                }
            }
        }
    }
}