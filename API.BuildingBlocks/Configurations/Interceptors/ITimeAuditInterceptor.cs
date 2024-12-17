using API.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace API.BuildingBlocks.Configurations.Interceptors;

public sealed class TimeAuditInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is null) return base.SavingChanges(eventData, result);
        var entries = GetAuditEntityEntries(eventData.Context);

        UpdateTimeAuditProperties(entries);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (eventData.Context is null) return base.SavingChangesAsync(eventData, result, cancellationToken);
        var entries = GetAuditEntityEntries(eventData.Context);

        UpdateTimeAuditProperties(entries);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static List<EntityEntry<ITimeAuditable>> GetAuditEntityEntries(DbContext context)
    {
        var entities = context.ChangeTracker
            .Entries<ITimeAuditable>()
            .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified || x.HasChangedOwnedEntities())
            .ToList();

        return entities;
    }

    private static void UpdateTimeAuditProperties(List<EntityEntry<ITimeAuditable>> entities)
    {
        var actionTime = DateTime.UtcNow;
        entities.ForEach(x =>
        {
            if (x.State == EntityState.Added)
                x.Entity.CreatedOn = actionTime;

            if (x.State == EntityState.Added || x.State == EntityState.Modified || x.HasChangedOwnedEntities())
                x.Entity.ModifiedOn = actionTime;
        });
    }
}

internal static class EntityExtensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry is not null &&
            r.TargetEntry.Metadata.IsOwned() &&
            r.TargetEntry.State is EntityState.Added or EntityState.Modified);
}