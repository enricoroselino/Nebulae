using API.Modules.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Modules.Identity.Persistence;

public class AppIdentityDbContext : DbContext
{
    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users => Set<User>();
}