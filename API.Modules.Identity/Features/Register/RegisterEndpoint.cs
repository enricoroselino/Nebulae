using Ardalis.Result.AspNetCore;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace API.Modules.Identity.Features.Register;

public record RegisterRequest(string Username, string Password, string PasswordConfirm, string Email, string Fullname);

public class RegisterEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ModuleConfig.AuthGroup);

        group.MapPost("/register", async (
                ISender mediator,
                [FromBody] RegisterRequest dto,
                CancellationToken cancellationToken) =>
            {
                var command = new RegisterCommand(
                    dto.Username,
                    dto.Password,
                    dto.PasswordConfirm,
                    dto.Email,
                    dto.Fullname
                );

                var result = await mediator.Send(command, cancellationToken);
                return result.ToMinimalApiResult();
            })
            .WithSummary("Registers a new user by providing necessary credentials.")
            .WithDescription(
                "The Register endpoint allows new users to create an account by providing their desired credentials.");
    }
}