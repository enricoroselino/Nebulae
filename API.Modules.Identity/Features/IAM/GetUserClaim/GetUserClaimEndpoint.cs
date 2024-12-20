namespace API.Modules.Identity.Features.IAM.GetUserClaim;

public class GetUserClaimEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ModuleConfig.IamGroup);

        group.MapGet("/user-claims/{userId:guid}", async (
            ISender mediator,
            [FromRoute] Guid userId,
            CancellationToken cancellationToken) =>
        {
            var query = new GetUserClaimQuery(new UserId(userId));
            var result = await mediator.Send(query, cancellationToken);
            return result.ToMinimalApiResult();
        })
        .RequireAuthorization()
        .WithSummary("Get user claims")
        .WithSummary("Returns a list of user claims");
    }
}