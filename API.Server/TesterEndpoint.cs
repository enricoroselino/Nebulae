using API.Shared.Extensions;
using Carter;
using Shared;

namespace API.Server;

public class TesterEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/tester", () =>
        {
            var result = Result.Conflict("Run purrfectly");
            return result.ToMinimalApiResult();
        });
    }
}