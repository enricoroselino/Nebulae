using API.Modules.Identity.Models.Enums;

namespace API.Modules.Identity.Features.IAM.UserClaim.AddUserClaim;

public record AddUserClaimRequest(Guid UserId, ClaimTypeEnum ClaimType, string ClaimValue);

public class AddUserClaimEndpoint : ICarterModule
{
    private static string ClaimTypeValues => string.Join(", ", Enum.GetNames<ClaimTypeEnum>());
    
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ModuleConfig.IamGroup);

        group.MapPost("/users-claims", async (
            ISender mediator, 
            [FromBody] AddUserClaimRequest dto,
            CancellationToken cancellationToken) =>
        {
            var command = new AddUserClaimCommand(
                new UserId(dto.UserId),
                dto.ClaimType.ToString(),
                dto.ClaimValue
            );
            var result = await mediator.Send(command, cancellationToken);
            return result.ToMinimalApiResult();
        })
        .WithSummary("Add user claim")
        .WithDescription($"[{nameof(AddUserClaimRequest.ClaimType)}] Possible Values : {ClaimTypeValues}");;
    }
}