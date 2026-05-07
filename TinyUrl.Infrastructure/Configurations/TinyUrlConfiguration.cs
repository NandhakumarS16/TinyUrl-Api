using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TinyUrl.Domain.Entities;

namespace TinyUrl.Infrastructure.Data.Configurations;

public class TinyUrlConfiguration : IEntityTypeConfiguration<TinyUrlEntity>
{
    public void Configure(EntityTypeBuilder<TinyUrlEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .UseIdentityColumn();                    // SQL Server IDENTITY(1,1)

        builder.Property(e => e.OriginalUrl)
               .IsRequired()
               .HasColumnType("nvarchar(2048)");        // SQL Server: unicode, max URL length

        builder.Property(e => e.ShortCode)
               .IsRequired()
               .HasMaxLength(6)
               .HasColumnType("nvarchar(6)");

        builder.Property(e => e.IsPrivate)
               .IsRequired()
               .HasDefaultValue(false);

        builder.Property(e => e.Clicks)
               .IsRequired()
               .HasDefaultValue(0);

        builder.Property(e => e.CreatedAt)
               .IsRequired()
               .HasColumnType("datetime2")              // SQL Server: high-precision datetime
               .HasDefaultValueSql("GETUTCDATE()");    // SQL Server UTC default

        builder.HasIndex(e => e.ShortCode)
               .IsUnique()
               .HasDatabaseName("IX_TinyUrls_ShortCode");

        builder.ToTable("TinyUrls");
    }
}