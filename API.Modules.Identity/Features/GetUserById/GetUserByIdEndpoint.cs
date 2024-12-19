using Ardalis.Result.AspNetCore;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace API.Modules.Identity.Features.GetUserById;

public class GetUserByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ModuleConfig.IamGroup);

        group.MapGet("/users/{userId:guid}", async (
            ISender mediator,
            [FromRoute] Guid userId,
            CancellationToken cancellationToken) =>
        {
            var query = new GetUserByIdQuery(new UserId(userId));
            var result = await mediator.Send(query, cancellationToken);
            return result.ToMinimalApiResult();
        });
    }
}