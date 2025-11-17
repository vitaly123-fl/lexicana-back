using MediatR;
using lexicana.Database;
using FirebaseAdmin.Auth;
using lexicana.Endpoints;
using lexicana.UserFolder.Enums;
using Microsoft.EntityFrameworkCore;

namespace lexicana.Authorization.Queries.GetFirebaseToken;

public record GetFirebaseTokenRequest(string Email): IHttpRequest<string>;

public class Handler: IRequestHandler<GetFirebaseTokenRequest, Response<string>>
{
    private readonly FirebaseAuth _firebaseAuth;
    private readonly ApplicationDbContext _context;

    public Handler(FirebaseAuth firebaseAuth, ApplicationDbContext context)
    {
        _context = context;
        _firebaseAuth = firebaseAuth;
    }
        
    public async Task<Response<string>> Handle(GetFirebaseTokenRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(
            x => x.Email == request.Email 
            && x.Provider == FirebaseProviderEnum.Password
        );
        
        if (user is null)
            return FailureResponses.NotFound<string>("User not found");
        
        string customToken = await _firebaseAuth.CreateCustomTokenAsync(user.FirebaseId);
        return SuccessResponses.Ok(customToken);
    }
}