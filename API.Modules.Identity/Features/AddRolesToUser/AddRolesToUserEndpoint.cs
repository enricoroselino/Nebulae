namespace API.Modules.Identity.Features.AddRolesToUser;

public record AddRoleToUserRequest(Guid UserId, List<Guid> RoleIds);

public class AddRolesToUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ModuleConfig.IamGroup);

        group.MapPatch("/user-roles", async (
                ISender sender,
                [FromBody] AddRoleToUserRequest dto,
                CancellationToken cancellationToken) =>
            {
                var command = new AddRolesToUserCommand(
                    new UserId(dto.UserId),
                    dto.RoleIds.Select(x => new RoleId(x)).ToList()
                );

                var result = await sender.Send(command, cancellationToken);
                return result.ToMinimalApiResult();
            })
            .WithSummary("Assign one or more roles from an existing user.")
            .WithDescription(
                "Assigning one or more roles to an existing user. It expects a list of role IDs and assigns them to the specified user. If the user already has the roles, they are skipped.");
    }
}