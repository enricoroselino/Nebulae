using Ardalis.Result.AspNetCore;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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
            })
            .WithSummary("Adds new roles to the system.")
            .WithDescription(
                "The endpoint accepts a list of role names and creates the roles if they do not already exist. Upon successful creation, the roles are added to the system and available for assignment to users. If the roles already exist, they are skipped.");
    }
}