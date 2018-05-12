using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NoName.Data.Configuration;
using NoName.Data.Entity;
using System;

namespace NoName.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Member> Member { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MemberConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public override EntityEntry Add(object entity)
        {
            var baseEntity = entity as BaseEntity;
            if(baseEntity != null)
            {
                baseEntity.CreateDate = DateTime.UtcNow;
            }

            return base.Add(entity);
        }
    }
}