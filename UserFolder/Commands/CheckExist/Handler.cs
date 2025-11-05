using MediatR;
using lexicana.Database;
using lexicana.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lexicana.UserFolder.Commands.CheckExist;

public record CheckExistUserRequest([FromBody] CheckExistUserBody Body) : IHttpRequest<EmptyValue>;

public record CheckExistUserBody(string Email);

public class Handler: IRequestHandler<CheckExistUserRequest, Response<EmptyValue>>
{
    private readonly ApplicationDbContext _context;

    public Handler(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Response<EmptyValue>> Handle(CheckExistUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x=> x.Email == request.Body.Email, cancellationToken);

        if (user is null)
            return FailureResponses.NotFound("User not exist");
        
        return SuccessResponses.Ok();
    }
}