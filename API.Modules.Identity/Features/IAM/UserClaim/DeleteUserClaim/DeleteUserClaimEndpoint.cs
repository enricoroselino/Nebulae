namespace API.Modules.Identity.Features.IAM.UserClaim.DeleteUserClaim;

public record DeleteUserClaimRequest(List<int> UserClaimIds);

public class DeleteUserClaimEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ModuleConfig.IamGroup);

        group.MapDelete("/users-claims", async (
                ISender mediator,
                [FromBody] DeleteUserClaimRequest dto,
                CancellationToken cancellationToken) =>
            {
                var command = new DeleteUserClaimCommand(dto.UserClaimIds.Select(x => new UserClaimId(x)).ToList());
                var result = await mediator.Send(command, cancellationToken);
                return result.ToMinimalApiResult();
            })
            .WithSummary("Delete user claims based on their unique identifiers.")
            .WithDescription(
                "Delete one or more user claims by providing a list of UserClaimIds in the request body. The request body should include a list of integer IDs representing the user claims to be deleted.");
    }
}