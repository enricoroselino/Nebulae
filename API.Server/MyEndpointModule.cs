using API.Shared.Extensions;
using Carter;
using Shared;

namespace API.Server;

public class MyEndpointModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", () =>
        {
            var result = Result.BadRequest("Hello World!");
            return result.ToMinimalApiResult();
        });
    }
}