using System.Text.RegularExpressions;

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