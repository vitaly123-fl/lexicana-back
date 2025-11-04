using MediatR;
using lexicana.Database;
using FirebaseAdmin.Auth;
using lexicana.Endpoints;
using lexicana.Authorization.Services;

namespace lexicana.UserFolder.Commands.DeleteAccount;

public record DeleteAccountRequest() : IHttpRequest<EmptyValue>;

public class Handler: IRequestHandler<DeleteAccountRequest, Response<EmptyValue>>
{
    private readonly AuthService _authService;
    private readonly FirebaseAuth _firebaseAuth;
    private readonly ApplicationDbContext _context;


    public Handler(AuthService authService, ApplicationDbContext context, FirebaseAuth firebaseAuth)
    {
        _context = context;
        _authService = authService;
        _firebaseAuth = firebaseAuth;
    }
    
    public async Task<Response<EmptyValue>> Handle(DeleteAccountRequest request, CancellationToken cancellationToken)
    {
        var userId = _authService.GetCurrentUserId();
        var user = await _context.Users.FindAsync(userId);

        if (user is null)
            return FailureResponses.NotFound("User not found");
        
        await _firebaseAuth.DeleteUserAsync(user.FirebaseId);
        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
        
        return SuccessResponses.Ok();
    }
}