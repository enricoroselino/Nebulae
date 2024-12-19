using API.Shared.Models.CQRS;
using Ardalis.Result;
using FluentValidation;

namespace API.Modules.Identity.Features.DeleteRoleFromUser;

public class DeleteRoleFromUserCommandValidator : AbstractValidator<DeleteRoleFromUserCommand>
{
    public DeleteRoleFromUserCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
        RuleForEach(c => c.RoleIds).NotEmpty();
    }
}