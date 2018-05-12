using NoName.Data.Context;
using NoName.Data.Entity;
using System;
using System.Linq;

namespace NoName.Data.DbInitializer
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (!context.Member.Any())
            {
                var member = new Member();
                member.MemberIdentifier = "905384220149";
                member.Password = "123";
                member.FirstName = "Tanju";
                member.LastName = "Yayak";
                member.CreateDate = DateTime.UtcNow;

                context.Member.Add(member);
                context.SaveChanges();
            }
        }
    }
}