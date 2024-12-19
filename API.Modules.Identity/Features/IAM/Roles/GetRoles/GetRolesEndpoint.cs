namespace API.Modules.Identity.Features.IAM.Roles.GetRoles;

public class GetRolesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ModuleConfig.IamGroup);

        group.MapGet("/roles", async (
                ISender mediator,
                CancellationToken cancellationToken) =>
            {
                var query = new GetRolesQuery();
                var result = await mediator.Send(query, cancellationToken);
                return result.ToMinimalApiResult();
            })
            .WithSummary("Retrieve a list of all available roles.")
            .WithDescription("Retrieve a list of all available roles.");
    }
}