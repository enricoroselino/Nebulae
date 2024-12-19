using System.Text.RegularExpressions;
using Ardalis.Result.AspNetCore;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace API.Modules.Identity.Features.GetUsers;

public class GetUsersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ModuleConfig.IamGroup);

        group.MapGet("/users", async (
            ISender mediator,
            CancellationToken cancellationToken) =>
        {
            var query = new GetUsersQuery();
            var result = await mediator.Send(query, cancellationToken);
            return result.ToMinimalApiResult();
        });
    }
}