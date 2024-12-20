namespace API.Modules.Identity.Features.IAM.GetUserById;

public class GetUserByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ModuleConfig.IamGroup);

        group.MapGet("/users/{userId:guid}", async (
                ISender mediator,
                [FromRoute] Guid userId,
                CancellationToken cancellationToken) =>
            {
                var query = new GetUserByIdQuery(new UserId(userId));
                var result = await mediator.Send(query, cancellationToken);
                return result.ToMinimalApiResult();
            })
            .WithSummary("Retrieve details of a specific user by their unique ID.")
            .WithDescription("Retrieve details of a specific user by their unique ID.");
    }
}