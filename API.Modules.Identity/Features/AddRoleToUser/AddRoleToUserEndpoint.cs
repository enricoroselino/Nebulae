using Ardalis.Result.AspNetCore;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace API.Modules.Identity.Features.AddRoleToUser;

public record AddRoleToUserRequest(Guid UserId, List<string> RoleNames);

public class AddRoleToUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ModuleConfig.IamGroup);

        group.MapPatch("/user-role", async (
            ISender sender,
            [FromBody] AddRoleToUserRequest dto,
            CancellationToken cancellationToken) =>
        {
            var command = new AddRoleToUserCommand(new UserId(dto.UserId), dto.RoleNames);
            var result = await sender.Send(command, cancellationToken);
            return result.ToMinimalApiResult();
        });
    }
}