using NoName.Enum;
using System;

namespace NoName.Data.Entity
{
    public class RefreshToken : BaseEntity
    {
        public int MemberId { get; set; }

        public Member Member { get; set; }

        public ChannelEnum Channel { get; set; }

        public string RefreshTokenInfo { get; set; }

        public DateTime RefreshTokenExpireDate { get; set; }
    }
}