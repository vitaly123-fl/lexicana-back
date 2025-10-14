using MediatR;
using FirebaseAdmin.Auth;
using lexicana.Authorization.Services;
using lexicana.Database;
using lexicana.Endpoints;
using lexicana.UserFolder.ProviderFolder.Enums;
using Microsoft.EntityFrameworkCore;

namespace lexicana.UserFolder.Commands.UpdatePassword;

public record UpdateUserPasswordRequest(string NewPassword) : IHttpRequest<EmptyValue>;

public class Handler : IRequestHandler<UpdateUserPasswordRequest, Response<EmptyValue>>
{
    private readonly AuthService _authService;
    private readonly FirebaseAuth _firebaseAuth;
    private readonly ApplicationDbContext _context;
    
    public Handler(FirebaseAuth firebaseAuth, AuthService authService, ApplicationDbContext context)
    {
        _context = context;
        _authService = authService;
        _firebaseAuth = firebaseAuth;
    }

    public async Task<Response<EmptyValue>> Handle(UpdateUserPasswordRequest request, CancellationToken cancellationToken)
    {
        var userId = _authService.GetCurrentUserId();
        
        if (userId is null)
            return FailureResponses.BadRequest("Your session is invalid. Please login again.");

        var user = await _context.Users
            .Include(x => x.Providers)
            .FirstOrDefaultAsync(x=>x.Id == userId);

        if (user is null)
            return FailureResponses.NotFound("User not found");

        var providerData = user.Providers.FirstOrDefault(x => x.Provider == FirebaseProviderEnum.Password);

        if (providerData is null)
            return FailureResponses.BadRequest("Provider 'password' not found");
        
        await _firebaseAuth.UpdateUserAsync(new UserRecordArgs
        {
            Uid = providerData.FirebaseId,
            Password = request.NewPassword
        });
        
        return SuccessResponses.Ok();
    }
}