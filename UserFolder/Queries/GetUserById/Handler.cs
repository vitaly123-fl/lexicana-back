using MediatR;
using lexicana.Database;
using lexicana.Endpoints;
using lexicana.Common.Enums;
using lexicana.Authorization.Services;

namespace lexicana.UserFolder.Queries.GetUserById;

public record GetUserRequest() : IHttpRequest<GetUserResponse>;

public record GetUserResponse(
    Guid Id, 
    string Email,
    string PhotoUrl,
    string DisplayName,
    Language? Language,
    string Provider
);

public class Handler: IRequestHandler<GetUserRequest, Response<GetUserResponse>>
{
    private readonly AuthService _authService;
    private readonly ApplicationDbContext _context;
    
    public Handler(ApplicationDbContext context, AuthService authService)
    {
        _context = context;
        _authService = authService;
    }
    
    public async Task<Response<GetUserResponse>> Handle(GetUserRequest request, CancellationToken cancellationToken)
    {
        var userId = _authService.GetCurrentUserId();
        var user = await _context.Users.FindAsync(userId, cancellationToken);
        
        if (user == null) return FailureResponses.NotFound<GetUserResponse>("User not found");
        
        var userDto = new GetUserResponse(
            user.Id, 
            user.Email,
            user.PhotoUrl,
            user.DisplayName,
            user.Language,
            user.Provider
        );
        
        return SuccessResponses.Ok(userDto);
    }
}