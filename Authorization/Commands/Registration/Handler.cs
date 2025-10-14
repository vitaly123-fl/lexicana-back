using MediatR;
using lexicana.Database;
using FirebaseAdmin.Auth;
using lexicana.Endpoints;
using Microsoft.AspNetCore.Mvc;
using lexicana.UserFolder.Entities;
using Microsoft.EntityFrameworkCore;
using lexicana.Authorization.Services;
using lexicana.UserFolder.ProviderFolder.Enums;
using UserProvider = lexicana.UserFolder.ProviderFolder.Entities.UserProvider;

namespace lexicana.Authorization.Commands.Registration;

public record RegisterUserRequest([FromBody] RegisterUserBody Body) : IHttpRequest<string>;
public record RegisterUserBody(string IdToken);

public class Handler : IRequestHandler<RegisterUserRequest, Response<string>>
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
        if (firebaseToken is null) return FailureResponses.BadRequest<string>("Your token is invalid for this project.");

        var firebaseUser = await _firebaseAuth.GetUserAsync(firebaseToken.Uid);
        if (firebaseUser is null) return FailureResponses.NotFound<string>("Firebase user not found.");

        var providerInfo = firebaseUser.ProviderData.FirstOrDefault();
        if (providerInfo is null) return FailureResponses.BadRequest<string>("Provider data is missing.");

        var firebaseId = firebaseUser.Uid;
        var provider = providerInfo.ProviderId;
        var email = providerInfo.Email ?? firebaseUser.Email;
        
        var existingUser = await _context.Users
            .Include(u => u.Providers)
            .FirstOrDefaultAsync(u => u.Email == email);

        if (existingUser is not null)
        {
            var providerExists = existingUser.Providers.Any(p => p.Provider == provider);

            if (providerExists) return FailureResponses.BadRequest<string>($"User with provider '{provider}' already exists.");

            var newProvider = new UserProvider
            {
                FirebaseId = firebaseId,
                Provider = provider,
                User = existingUser
            };

            _context.UserProviders.Add(newProvider);
            
            existingUser.PhotoUrl = GetUserAvatar(firebaseUser);
            existingUser.DisplayName = firebaseUser.DisplayName;
            
            await _context.SaveChangesAsync();

            var token = _jwtService.GenerateToken(existingUser.Id, existingUser.Email);
            return SuccessResponses.Ok(token);
        }

        var newUser = new User
        {
            Email = email,
            DisplayName = firebaseUser.DisplayName ?? "User",
            PhotoUrl = GetUserAvatar(firebaseUser)
        };

        var userProvider = new UserProvider
        {
            FirebaseId = firebaseId,
            Provider = provider,
            User = newUser
        };

        newUser.Providers.Add(userProvider);

        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();

        var newToken = _jwtService.GenerateToken(newUser.Id, newUser.Email);
        return SuccessResponses.Ok(newToken);
    }

    private string GetUserAvatar(UserRecord user)
    {
        var provider = user.ProviderData.First();
        
        if (provider.ProviderId == FirebaseProviderEnum.Google)
            return user.PhotoUrl;

        var name = !string.IsNullOrWhiteSpace(user.DisplayName)
            ? user.DisplayName
            : provider.Email ?? user.Email;

        var encodedName = Uri.EscapeDataString(name);
        return $"https://ui-avatars.com/api/?name={encodedName}&background=random&color=fff&size=256&rounded=true";
    }
}