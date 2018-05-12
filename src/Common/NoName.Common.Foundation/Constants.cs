using System;

namespace NoName.Common.Foundation
{
    public static class Constants
    {
        public static class GrantType
        {
            public static readonly string Password = nameof(Password);
            public static readonly string Refresh_Token = nameof(Refresh_Token);
        }

        public static class Claim
        {
            public static readonly string UserId = nameof(UserId);
            public static readonly string Roles = nameof(Roles);
        }

        public static class Exception
        {
            public static readonly string MEMBER_NOT_FOUND = nameof(MEMBER_NOT_FOUND);
            public static readonly string INVALID_GRANT_TYPE = nameof(INVALID_GRANT_TYPE);
            public static readonly string REFRESH_TOKEN_NOT_FOUND = nameof(REFRESH_TOKEN_NOT_FOUND);
            public static readonly string REFRESH_TOKEN_EXPIRED = nameof(REFRESH_TOKEN_EXPIRED);
        }

        public static class Common
        {
            public static readonly string DefaultConnection = nameof(DefaultConnection);
        }
    }
}
