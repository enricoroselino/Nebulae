using Ardalis.Result.AspNetCore;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace API.Modules.Identity.Features.DeleteRole;

public class DeleteRoleEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ModuleConfig.IamGroup);

        group.MapDelete("/roles/{roleId:guid}", async (
            ISender mediator,
            [FromRoute] Guid roleId,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteRoleCommand(new RoleId(roleId));
            var result = await mediator.Send(command, cancellationToken);
            return result.ToMinimalApiResult();
        })
        .WithSummary("Deletes an existing role from the system.")
        .WithDescription("Remove a role is identified by its unique role ID. If the role is currently assigned to any users, it will be removed from their profiles, and the role will no longer be available for future assignments.");
    }
}