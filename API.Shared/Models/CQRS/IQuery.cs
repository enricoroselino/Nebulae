using MediatR;

namespace API.Shared.Models.CQRS;

public interface IQuery<out TResponse> : IRequest<TResponse> where TResponse : notnull;