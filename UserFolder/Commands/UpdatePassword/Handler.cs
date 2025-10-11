using MediatR;
using FirebaseAdmin.Auth;
using lexicana.Authorization.Services;
using lexicana.Endpoints;

namespace lexicana.UserFolder.Commands.UpdatePassword;

public record UpdateUserPasswordRequest(string NewPassword) : IHttpRequest<EmptyValue>;

public class Handler : IRequestHandler<UpdateUserPasswordRequest, Response<EmptyValue>>
{
    private readonly AuthService _authService;
    private readonly FirebaseAuth _firebaseAuth;
    
    public Handler(FirebaseAuth firebaseAuth, AuthService authService)
    {
        _authService = authService;
        _firebaseAuth = firebaseAuth;
    }

    public async Task<Response<EmptyValue>> Handle(UpdateUserPasswordRequest request, CancellationToken cancellationToken)
    {
        var firebaseId = _authService.GetCurrentUserFirebaseId();

        if (firebaseId is null)
        {
            return FailureResponses.BadRequest("Your session is invalid. Please login again.");
        }
        
        await _firebaseAuth.UpdateUserAsync(new UserRecordArgs
        {
            Uid = firebaseId,
            Password = request.NewPassword
        });
        
        return SuccessResponses.Ok();
    }
}