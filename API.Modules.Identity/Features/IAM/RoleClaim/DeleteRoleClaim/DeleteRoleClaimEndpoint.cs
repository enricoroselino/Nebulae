namespace API.Modules.Identity.Features.IAM.RoleClaim.DeleteRoleClaim;

public record DeleteRoleClaimRequest(List<int> RoleClaimIds);

public class DeleteRoleClaimEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ModuleConfig.IamGroup);

        group.MapDelete("/roles-claims", async (
                ISender mediator,
                [FromBody] DeleteRoleClaimRequest dto,
                CancellationToken cancellationToken) =>
            {
                var command = new DeleteRoleClaimCommand(dto.RoleClaimIds.Select(x => new RoleClaimId(x)).ToList());
                var result = await mediator.Send(command, cancellationToken);
                return result.ToMinimalApiResult();
            })
            .WithSummary("Delete role claims based on their unique identifiers.")
            .WithDescription(
                "Delete one or more role claims by providing a list of RoleClaimIds in the request body. The request body should include a list of integer IDs representing the role claims to be deleted.");
    }
}