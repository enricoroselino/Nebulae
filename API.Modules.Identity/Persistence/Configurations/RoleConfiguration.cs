namespace API.Modules.Identity.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(x => x.Value, x => new RoleId(x))
            .ValueGeneratedNever();

        builder.Property(c => c.Name)
            .HasMaxLength(50)
            .IsRequired();
    }
}