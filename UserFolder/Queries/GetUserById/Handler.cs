using MediatR;
using lexicana.Database;
using lexicana.Endpoints;
using lexicana.Authorization.Services;
using lexicana.Common.Enums;

namespace lexicana.UserFolder.Queries.GetUserById;

public record GetUserRequest() : IHttpRequest<GetUserResponse>;

public record GetUserResponse(
    Guid Id, 
    string Email,
    string DisplayName,
    string PhotoUrl,
    string Provider,
    Language? Language
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
        
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return FailureResponses.NotFound<GetUserResponse>("User not found");

        var userDto = new GetUserResponse(
            user.Id, 
            user.Email,
            user.DisplayName,
            user.PhotoUrl,
            user.Provider,
            user.Language
        );
        
        return SuccessResponses.Ok(userDto);
    }
}