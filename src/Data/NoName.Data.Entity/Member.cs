using System.Collections.Generic;

namespace NoName.Data.Entity
{
    public class Member: BaseEntity
    {
        /// <summary>
        /// Could be phone number, email etc.
        /// </summary>
        public string MemberIdentifier { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IList<RefreshToken> RefreshTokens { get; set; }
    }
}