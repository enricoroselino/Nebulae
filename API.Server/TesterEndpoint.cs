using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Carter;

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