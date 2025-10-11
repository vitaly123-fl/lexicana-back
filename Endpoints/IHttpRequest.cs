using MediatR;

namespace lexicana.Endpoints;

public interface IHttpRequest<T> : IRequest<Response<T>>;

