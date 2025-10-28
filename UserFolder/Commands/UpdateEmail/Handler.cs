using MediatR;
using FirebaseAdmin.Auth;
using lexicana.Endpoints;
using lexicana.Authorization.Services;

namespace lexicana.UserFolder.Commands.UpdateEmail;

public record UpdateUserEmailRequest(string NewEmail) : IHttpRequest<EmptyValue>;

public class Handler : IRequestHandler<UpdateUserEmailRequest, Response<EmptyValue>>
{
    private readonly AuthService _authService;
    private readonly FirebaseAuth _firebaseAuth;
    
    public Handler(FirebaseAuth firebaseAuth, AuthService authService)
    {
        _authService = authService;
        _firebaseAuth = firebaseAuth;
    }

    public async Task<Response<EmptyValue>> Handle(UpdateUserEmailRequest request, CancellationToken cancellationToken)
    {
        //TODO винести це в сервіс
        var firebaseId = _authService.GetCurrentUserFirebaseId();

        if (firebaseId is null)
        {
            //TODO це потрібно кудись винести, бо я це не перший раз бачу
            return FailureResponses.BadRequest("Your session is invalid. Please login again.");
        }
        
        var user = await _firebaseAuth.GetUserAsync(firebaseId);
        var provider = user.ProviderId;

        // now only for password, need logic for google apple providers
        if (provider != "password")
        {
            return FailureResponses.BadRequest($"Cannot update email for provider {provider}.");
        }
        
        await _firebaseAuth.UpdateUserAsync(new UserRecordArgs
        {
            Uid = firebaseId,
            Email = request.NewEmail,
            EmailVerified = false
        });
        
        return SuccessResponses.Ok();
    }
}
