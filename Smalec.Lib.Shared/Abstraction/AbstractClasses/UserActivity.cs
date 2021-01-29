using System;

namespace Smalec.Lib.Shared.Abstraction.AbstractClasses
{
    public abstract class UserActivity
    {
        public string UserUuid { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}