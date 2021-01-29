using System;

namespace Smalec.Service.Friendship.Models.Relations
{
    public class FRIENDS_WITH
    {
        public static string InvitationSentByField = nameof(InvitationSentBy);
        public static string InvitationSentOnField = nameof(InvitationSentOn);
        public static string InvitationAcceptedOnField = nameof(InvitationAcceptedOn);
        public static string IsAcceptedField = nameof(IsAccepted);

        public string InvitationSentBy { get; set; }
        public DateTime InvitationSentOn { get; set; }
        public DateTime InvitationAcceptedOn { get; set; }
        public bool IsAccepted { get; set; }
    }
}
