namespace API.Modules.Identity.Features.IAM.GetUsers;

public class GetUsersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ModuleConfig.IamGroup);

        group.MapGet("/users", async (
            ISender mediator,
            CancellationToken cancellationToken) =>
        {
            var query = new GetUsersQuery();
            var result = await mediator.Send(query, cancellationToken);
            return result.ToMinimalApiResult();
        })
        .RequireAuthorization()
        .WithSummary("Retrieve a list of users.")
        .WithDescription("Returns a list of users, optionally filtered and paginated.");
    }
}