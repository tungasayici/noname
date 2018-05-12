using Microsoft.EntityFrameworkCore;
using NoName.Core.Repository;
using NoName.Data.Entity;

namespace NoName.Repository
{
    public class MemberRepository : GenericRepository<Member>, IMemberRepository
    {
        public MemberRepository(DbContext context) : base(context)
        {

        }
    }
}