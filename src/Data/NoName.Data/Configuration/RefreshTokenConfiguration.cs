using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoName.Data.Entity;

namespace NoName.Data.Configuration
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshToken");
            builder.HasKey(k => k.Id);
            builder.HasOne(o => o.Member).WithMany(w=> w.RefreshTokens).HasForeignKey(a => a.MemberId).IsRequired();
        }
    }
}