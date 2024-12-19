namespace API.Modules.Identity.Persistence.Configurations;

public class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
{
    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasConversion(c => c.Value, c => new RoleClaimId(c));

        builder.Property(c => c.ClaimType)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.ClaimValue)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasOne(c => c.Role)
            .WithMany(cc => cc.RoleClaims)
            .HasForeignKey(c => c.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}