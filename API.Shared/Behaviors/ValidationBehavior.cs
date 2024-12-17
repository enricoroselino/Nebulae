using API.Shared.Models.CQRS;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace API.Shared.Behaviors;

internal sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any()) return await next();
        var context = new ValidationContext<TRequest>(request);

        var failures = await Validate(_validators, context, cancellationToken);
        if (failures.Count > 0) throw new ValidationException(failures);
        return await next();
    }

    private static async Task<List<ValidationFailure>> Validate(
        IEnumerable<IValidator<TRequest>> validators,
        ValidationContext<TRequest> context,
        CancellationToken cancellationToken)
    {
        var validatorTasks = validators
            .Select(async x => await x.ValidateAsync(context, cancellationToken));

        var validationResults = await Task.WhenAll(validatorTasks);
        var failures = validationResults
            .Where(r => r.Errors.Count > 0)
            .SelectMany(r => r.Errors)
            .ToList();

        return failures;
    }
}