namespace API.Modules.Identity.Features.IAM.UserRoles.DeleteUserRoles;

public record DeleteRoleFromUserRequest(Guid UserId, List<Guid> RoleIds);

public class DeleteUserRolesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ModuleConfig.IamGroup);

        group.MapDelete("/user-roles", async (
                ISender mediator,
                [FromBody] DeleteRoleFromUserRequest dto,
                CancellationToken cancellationToken) =>
            {
                var command = new DeleteUserRolesCommand(
                    new UserId(dto.UserId),
                    dto.RoleIds.Select(x => new RoleId(x)).ToList()
                );
                
                var result = await mediator.Send(command, cancellationToken);
                return result.ToMinimalApiResult();
            })
            .WithSummary("Removes one or more roles from an existing user.")
            .WithDescription(
                "Remove specified roles from a user. It expects a list of role IDs and removes them from the user’s account. If the user does not have the roles, no action is taken.");
    }
}