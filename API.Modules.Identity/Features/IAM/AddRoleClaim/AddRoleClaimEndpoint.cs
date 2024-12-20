using API.Modules.Identity.Models.Enums;

namespace API.Modules.Identity.Features.IAM.AddRoleClaim;

public record AddRoleClaimRequest(Guid RoleId, ClaimType ClaimType, string ClaimValue);

public class AddRoleClaimEndpoint : ICarterModule
{
    private static string ClaimTypeValues => string.Join(", ", Enum.GetNames<ClaimType>());

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ModuleConfig.IamGroup);

        group.MapPost("/roles-claims", async (
                ISender mediator,
                [FromBody] AddRoleClaimRequest dto,
                CancellationToken cancellationToken) =>
            {
                var command = new AddRoleClaimCommand(
                    new RoleId(dto.RoleId),
                    dto.ClaimType.ToString(),
                    dto.ClaimValue
                );
                var result = await mediator.Send(command, cancellationToken);
                return result.ToMinimalApiResult();
            })
            .WithSummary("Add role claim")
            .WithDescription($"[{nameof(AddRoleClaimRequest.ClaimType)}] Possible Values : {ClaimTypeValues}");
    }
}