using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API.Shared.Extensions;

public static class DatabaseExtension
{
    public static async Task MigrateDatabase<TDbContext>(this WebApplication app) where TDbContext : DbContext
    {
        if (!app.Environment.IsDevelopment()) return;

        await using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}