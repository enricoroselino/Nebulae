namespace API.Modules.Identity.Persistence.Configurations;

public class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
{
    public void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasConversion(c => c.Value, c => new UserClaimId(c));

        builder.Property(c => c.ClaimType)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.ClaimValue)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasOne(c => c.User)
            .WithMany(cc => cc.UserClaims)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}