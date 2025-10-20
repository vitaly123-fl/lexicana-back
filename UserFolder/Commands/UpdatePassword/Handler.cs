using MediatR;
using lexicana.Database;
using FirebaseAdmin.Auth;
using lexicana.Endpoints;
using Microsoft.AspNetCore.Mvc;
using lexicana.Authorization.Services;
using lexicana.UserFolder.Enums;

namespace lexicana.UserFolder.Commands.UpdatePassword;

public record UpdateUserPasswordRequest([FromBody] UpdateUserPasswordBody Body) : IHttpRequest<EmptyValue>;

public record UpdateUserPasswordBody(
    string NewPassword
);

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

        var user = await _context.Users.FindAsync(userId);

        if (user is null)
            return FailureResponses.NotFound("User not found");
        
        if (user.Provider !=  FirebaseProviderEnum.Password)
            return FailureResponses.BadRequest("Provider 'password' not found");
        
        await _firebaseAuth.UpdateUserAsync(new UserRecordArgs
        {
            Uid = user.FirebaseId,
            Password = request.Body.NewPassword
        });
        
        return SuccessResponses.Ok();
    }
}