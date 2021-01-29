using Neo4j.Driver;
using Smalec.Service.Friendship.Abstraction;
using Smalec.Service.Friendship.Models.Nodes;
using Smalec.Service.Friendship.Models.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smalec.Service.Friendship.Repositories
{
    public class FriendshipRepository : IFriendshipRepository
    {
        private readonly IDriver _db;

        public FriendshipRepository(IDriver db)
        {
            _db = db;
        }

        public async Task AcceptFriend(string currentUser, string userToAccept)
        {
            var cql = $@"
                MATCH 
                    (u1:{nameof(User)} {{{User.UuidField}: ${nameof(userToAccept)}}}) 
                    -[f:FRIENDS_WITH]- 
                    (u2:{nameof(User)} {{{User.UuidField}: ${nameof(currentUser)}}}) 
                SET 
                    f.IsAccepted = true";

            var session = _db.AsyncSession();

            try
            {
                await session.RunAsync(cql, new { currentUser, userToAccept });
            }
            catch
            {
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task AddFriend(string currentUser, string userToAdd)
        {
            var currentDate = DateTime.Now.ToString("s");
            var minDate = DateTime.MinValue.ToString("s");

            var cql = $@"
                MERGE (u1:{nameof(User)} {{{User.UuidField}: ${nameof(currentUser)}}})
                MERGE (u2:{nameof(User)} {{{User.UuidField}: ${nameof(userToAdd)}}})
                MERGE (u1)
                        -[:{nameof(FRIENDS_WITH)}
                        {{
                                {FRIENDS_WITH.InvitationSentOnField} : datetime(${nameof(currentDate)}),
                                {FRIENDS_WITH.InvitationAcceptedOnField}: datetime(${nameof(minDate)}),
                                {FRIENDS_WITH.IsAcceptedField}: false,
                                {FRIENDS_WITH.InvitationSentByField}: ${nameof(currentUser)}
                        }}]->
                        (u2);";

            var session = _db.AsyncSession();

            try
            {
                await session.RunAsync(cql, new { currentUser, userToAdd, currentDate, minDate });
            }
            catch
            {
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task<FRIENDS_WITH> GetFriendshipInfo(string currentUser, string userToCheck)
        {
            var cql = $@"
                MATCH
                    (:{nameof(User)} {{{User.UuidField}: ${nameof(currentUser)} }})
                    -[f:{nameof(FRIENDS_WITH)}]-
                    (:{nameof(User)} {{{User.UuidField}: ${nameof(userToCheck)}}})
                RETURN
                    f.{FRIENDS_WITH.IsAcceptedField} as {FRIENDS_WITH.IsAcceptedField}, 
                    f.{FRIENDS_WITH.InvitationSentOnField} as {FRIENDS_WITH.InvitationSentOnField}, 
                    f.{FRIENDS_WITH.InvitationAcceptedOnField} as {FRIENDS_WITH.InvitationAcceptedOnField},
                    f.{FRIENDS_WITH.InvitationSentByField} as {FRIENDS_WITH.InvitationSentByField}";

            var session = _db.AsyncSession();

            try
            {
                var dbResult = await session.RunAsync(cql, new { currentUser, userToCheck });
                return await dbResult.SingleAsync(x => new FRIENDS_WITH
                {
                    IsAccepted = x[FRIENDS_WITH.IsAcceptedField].As<bool>(),
                    InvitationAcceptedOn = DateTime.Now,
                    InvitationSentOn = DateTime.Now,
                    InvitationSentBy = x[FRIENDS_WITH.InvitationSentByField].As<string>()
                });
            }
            catch(InvalidOperationException)
            {
                return null;
            }
            catch
            {
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task<IEnumerable<string>> GetUserFriendsUuids(string currentUserUuid)
        {
            var cql = $@"
                MATCH 
                    (:{nameof(User)} {{{User.UuidField}: ${nameof(currentUserUuid)}}}) 
                    -[:{nameof(FRIENDS_WITH)}]- 
                    (u:{nameof(User)})
                RETURN 
                    u.Uuid as Uuid;";

            IEnumerable<string> result = Enumerable.Empty<string>();

            var session = _db.AsyncSession();

            try
            {
                var dbResult = await session.RunAsync(cql, new { currentUserUuid });
                result = await dbResult.ToListAsync(x => x[User.UuidField].As<string>());
            }
            catch
            {
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }

            return result;
        }

        public async Task RejectFriend(string currentUser, string userToReject)
        {
            var cql = $@"
                MATCH 
                    (u1:{nameof(User)} {{{User.UuidField}: ${nameof(userToReject)}}}) 
                    -[f:FRIENDS_WITH]- 
                    (u2:{nameof(User)} {{{User.UuidField}: ${nameof(currentUser)}}}) 
                DELETE f;";

            var session = _db.AsyncSession();

            try
            {
                await session.RunAsync(cql, new { currentUser, userToReject });
            }
            catch
            {
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
        }
    }
}
