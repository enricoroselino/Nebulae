using API.Modules.Identity.Models.Enums;

namespace API.Modules.Identity.Features.IAM.AddUserClaim;

public record AddUserClaimRequest(Guid UserId, ClaimTypeEnum ClaimTypeEnum, string ClaimValue);

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
                    dto.ClaimTypeEnum.ToString(),
                    dto.ClaimValue
                );
                var result = await mediator.Send(command, cancellationToken);
                return result.ToMinimalApiResult();
            })
            .WithSummary("Add user claim")
            .WithDescription($"[{nameof(AddUserClaimRequest.ClaimTypeEnum)}] Possible Values : {ClaimTypeValues}");
        ;
    }
}