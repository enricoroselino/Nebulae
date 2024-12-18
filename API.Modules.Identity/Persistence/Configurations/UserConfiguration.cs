using API.Modules.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Modules.Identity.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(c => c.Id)
            .IsClustered(false);

        builder.Property(c => c.Id)
            .HasConversion(x => x.Value, x => new UserId(x))
            .ValueGeneratedNever();

        builder.HasIndex(c => c.CreatedOn)
            .IsClustered();

        builder.Property(c => c.Username)
            .HasMaxLength(32)
            .IsRequired();
        builder.Property(c => c.Email)
            .HasMaxLength(320)
            .IsRequired();
        builder.Property(c => c.PasswordHash)
            .IsRequired();
        builder.Property(c => c.CompatId)
            .IsRequired(false);

        builder.Property(c => c.Fullname)
            .HasMaxLength(70)
            .IsRequired();

        builder.HasMany(u => u.UserRoles)
            .WithOne(ur => ur.User)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}