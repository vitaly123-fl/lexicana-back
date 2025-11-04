using MediatR;
using lexicana.Database;
using FirebaseAdmin.Auth;
using lexicana.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lexicana.Authorization.Services;

namespace lexicana.Authorization.Commands.Login;

public record LoginUserRequest([FromBody] LoginUserBody Body) : IHttpRequest<string>;

public record LoginUserBody(string IdToken);

public class Handler: IRequestHandler<LoginUserRequest, Response<string>>
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
    
    public async Task<Response<string>> Handle(LoginUserRequest request, CancellationToken cancellationToken)
    {
        var firebaseToken = await _firebaseAuth.VerifyIdTokenAsync(request.Body.IdToken, cancellationToken);

        if (firebaseToken is null)
            return FailureResponses.BadRequest<string>("Token invalid for this project");

        var user = await _context.Users.FirstOrDefaultAsync(x=>x.FirebaseId == firebaseToken.Uid, cancellationToken);
        
        if (user is null)
            return FailureResponses.NotFound<string>("User not found. Please sign up.");

        var token = _jwtService.GenerateToken(user.Id, user.Email);

        return SuccessResponses.Ok(token);
    }
}