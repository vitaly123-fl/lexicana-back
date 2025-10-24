using MediatR;
using lexicana.Database;
using FirebaseAdmin.Auth;
using lexicana.Endpoints;
using Microsoft.AspNetCore.Mvc;
using lexicana.UserFolder.Entities;
using Microsoft.EntityFrameworkCore;
using lexicana.Authorization.Services;

namespace lexicana.Authorization.Commands.Registration;

public record RegisterUserRequest([FromBody] RegisterUserBody Body) : IHttpRequest<string>;

public record RegisterUserBody(string IdToken);

public class Handler: IRequestHandler<RegisterUserRequest, Response<string>>
{
    private readonly JWtService _jwtService;
    private readonly FirebaseAuth _firebaseAuth;
    private readonly ApplicationDbContext _context;
    
    public Handler(ApplicationDbContext context, FirebaseAuth firebaseAuth, JWtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
        _firebaseAuth = firebaseAuth;
    }
    
    public async Task<Response<string>> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var firebaseToken = await _firebaseAuth.VerifyIdTokenAsync(request.Body.IdToken);

        if (firebaseToken is null)
            return FailureResponses.BadRequest<string>("Your token invalid for this project");

        var firebaseUser = await _firebaseAuth.GetUserAsync(firebaseToken.Uid);
        
        if (firebaseUser is null)
            return FailureResponses.NotFound<string>("Firebase user not found");

        var user = await _context.Users.FirstOrDefaultAsync(x => x.FirebaseId == firebaseUser.Uid);
        
        if (user is not null)
            return FailureResponses.NotFound<string>("This user already exist. Please login.");

        var avatar = GetUserAvatar(firebaseUser);
        var provider = firebaseUser.ProviderData[0];
        
        var newUser = new User()
        {
            PhotoUrl = avatar,
            FirebaseId = firebaseUser.Uid,
            Provider = provider.ProviderId,
            Email = provider.Email ?? firebaseUser.Email,
            DisplayName = firebaseUser.DisplayName ?? "User"
        };

        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
        
        var token = _jwtService.GenerateToken(newUser.Id, newUser.FirebaseId);

        return SuccessResponses.Ok(token);
    }

    private string GetUserAvatar(UserRecord user)
    {
        if (!string.IsNullOrWhiteSpace(user.PhotoUrl))
            return user.PhotoUrl;

        var name = !string.IsNullOrWhiteSpace(user.DisplayName)
            ? user.DisplayName
            : user.Email ?? "User";

        var encodedName = Uri.EscapeDataString(name);

        var avatarUrl = $"https://ui-avatars.com/api/?name={encodedName}&background=random&color=fff&size=256&rounded=true";

        return avatarUrl;
    }
}