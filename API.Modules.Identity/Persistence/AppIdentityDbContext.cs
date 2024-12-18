namespace API.Modules.Identity.Persistence;

public class AppIdentityDbContext : DbContext
{
    private const string Schema = "Identity";
    private readonly IConfiguration _configuration;

    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options, IConfiguration configuration) :
        base(options)
    {
        _configuration = configuration;
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppIdentityDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        var conn = _configuration.GetConnectionString("AppDb");
        optionsBuilder.UseSqlServer(conn, options => options.MigrationsHistoryTable("__EFMigrationsHistory", Schema));
        optionsBuilder.UseLazyLoadingProxies();
    }
}