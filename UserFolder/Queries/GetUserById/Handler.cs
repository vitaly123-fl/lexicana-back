using MediatR;
using lexicana.Database;
using lexicana.Endpoints;
using lexicana.Common.Enums;
using Microsoft.EntityFrameworkCore;
using lexicana.Authorization.Services;

namespace lexicana.UserFolder.Queries.GetUserById;

public record GetUserRequest() : IHttpRequest<GetUserResponse>;

public record GetUserResponse(
    Guid Id, 
    string Email,
    string DisplayName,
    string PhotoUrl,
    Language? Language,
    List<string> Providers
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
        
        var user = await _context.Users
            .Include(x=>x.Providers)
            .FirstOrDefaultAsync(x=>x.Id == userId, cancellationToken);
        
        if (user == null) return FailureResponses.NotFound<GetUserResponse>("User not found");

        var providers = user.Providers.Select(x => x.Provider).ToList();
        
        var userDto = new GetUserResponse(
            user.Id, 
            user.Email,
            user.DisplayName,
            user.PhotoUrl,
            user.Language,
            providers
        );
        
        return SuccessResponses.Ok(userDto);
    }
}