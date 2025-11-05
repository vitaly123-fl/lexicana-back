using MediatR;
using lexicana.Database;
using FirebaseAdmin.Auth;
using lexicana.Endpoints;
using Microsoft.AspNetCore.Mvc;
using lexicana.UserFolder.Enums;
using Microsoft.EntityFrameworkCore;
using lexicana.Authorization.Services;

namespace lexicana.UserFolder.Commands.UpdateEmail;

public record UpdateUserEmailRequest([FromBody] UpdateUserEmailBody Body) : IHttpRequest<EmptyValue>;

public record UpdateUserEmailBody(string NewEmail);

public class Handler : IRequestHandler<UpdateUserEmailRequest, Response<EmptyValue>>
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

    public async Task<Response<EmptyValue>> Handle(UpdateUserEmailRequest request, CancellationToken cancellationToken)
    {
        var userId = _authService.GetCurrentUserId();
        var user = await _context.Users.FindAsync(userId);
        
        if (user is null)
            return FailureResponses.BadRequest("User not found.");
        
        var provider = user.Provider;
        // now only for password, need logic for google apple providers
        if (provider != FirebaseProviderEnum.Password)
            return FailureResponses.BadRequest($"Cannot update email for provider {provider}.");
        
        var emailExists = await _context.Users.AnyAsync(u => u.Email == request.Body.NewEmail && u.Id != userId);
        
        if (emailExists)
            return FailureResponses.BadRequest("This email is already in use.");
        
        await _firebaseAuth.UpdateUserAsync(new UserRecordArgs
        {
            Uid = user.FirebaseId,
            Email = request.Body.NewEmail,
            EmailVerified = false
        });

        user.Email = request.Body.NewEmail;
        await _context.SaveChangesAsync();
        
        return SuccessResponses.Ok();
    }
}
