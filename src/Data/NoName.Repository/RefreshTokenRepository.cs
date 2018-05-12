using Microsoft.EntityFrameworkCore;
using NoName.Core.Repository;
using NoName.Data.Entity;

namespace NoName.Repository
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(DbContext context) : base(context)
        {

        }
    }
}