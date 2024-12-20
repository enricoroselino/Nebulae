namespace API.Modules.Identity.Features.IAM.GetUserRoles;

public class GetUserRolesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ModuleConfig.IamGroup);

        group.MapGet("/user-roles/{userId:guid}", async (
            ISender mediator,
            [FromRoute] Guid userId,
            CancellationToken cancellationToken) =>
        {
            var query = new GetUserRolesQuery(new UserId(userId));
            var result = await mediator.Send(query, cancellationToken);
            return result.ToMinimalApiResult();
        })
        .RequireAuthorization()
        .WithSummary("Retrieve all roles assigned to a specific user.")
        .WithDescription("Fetch the list of roles assigned to a user identified by their unique userId. ");
    }
}