namespace API.Modules.Identity.Features.Auth.Login;

public record LoginEndpointRequest(string Username, string Password);

public class LoginEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ModuleConfig.AuthGroup);

        group.MapPost("/login", async (
                ISender mediator,
                [FromBody] LoginEndpointRequest dto,
                CancellationToken cancellationToken) =>
            {
                var command = new LoginCommand(dto.Username, dto.Password);
                var result = await mediator.Send(command, cancellationToken);
                return result.ToMinimalApiResult();
            })
            .WithSummary("Authenticates a user and returns an access token upon successful login.")
            .WithDescription(
                "The Login endpoint verifies user credentials (username and password) and generates a JSON Web Token (JWT) if authentication succeeds. This token is used for subsequent authenticated requests to secure endpoints.\nThe endpoint expects a JSON payload containing the username and password.");
    }
}