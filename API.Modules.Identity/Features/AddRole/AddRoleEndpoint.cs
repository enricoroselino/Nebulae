using Ardalis.Result.AspNetCore;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace API.Modules.Identity.Features.AddRole;

public class AddRoleEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ModuleConfig.IamGroup);

        group.MapPost("/role", async (
            ISender mediator,
            [FromQuery] string roleName,
            CancellationToken cancellationToken) =>
        {
            var command = new AddRoleCommand(roleName);
            var result = await mediator.Send(command, cancellationToken);
            return result.ToMinimalApiResult();
        });
    }
}