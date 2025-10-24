using MediatR;
using lexicana.Database;
using lexicana.Endpoints;
using Microsoft.AspNetCore.Mvc;
using lexicana.Authorization.Services;

namespace lexicana.UserFolder.Commands.UpdateName;

public record UpdateUserNameRequest([FromBody] UpdateUserNameBody Body) : IHttpRequest<EmptyValue>;

public record UpdateUserNameBody(string Name);

public class Handler: IRequestHandler<UpdateUserNameRequest, Response<EmptyValue>>
{
    private readonly AuthService _authService;
    private readonly ApplicationDbContext _context;
    
    public Handler(AuthService authService, ApplicationDbContext context)
    {
        _context = context;
        _authService = authService;
    }
    
    public async Task<Response<EmptyValue>> Handle(UpdateUserNameRequest request, CancellationToken cancellationToken)
    {
        var userId = _authService.GetCurrentUserId();
        
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return FailureResponses.NotFound("User not found");

        user.DisplayName = request.Body.Name;
        
        await _context.SaveChangesAsync();
        return SuccessResponses.Ok();
    }
}