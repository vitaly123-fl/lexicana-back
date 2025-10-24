using MediatR;
using lexicana.Database;
using lexicana.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lexicana.Authorization.Services;

namespace lexicana.UserFolder.Commands.UpdateFavoriteWord;

public record UpdateFavoriteWordRequest([FromBody] UpdateFavoriteWordBody Body) : IHttpRequest<EmptyValue>;

public record UpdateFavoriteWordBody(Guid Id);

public class Handler: IRequestHandler<UpdateFavoriteWordRequest, Response<EmptyValue>>
{
    private readonly AuthService _authService;
    private readonly ApplicationDbContext _context;

    public Handler(AuthService authService, ApplicationDbContext context)
    {
        _context = context;
        _authService = authService;
    }
    
    public async Task<Response<EmptyValue>> Handle(UpdateFavoriteWordRequest request, CancellationToken cancellationToken)
    {
        var userId = _authService.GetCurrentUserId();
        var wordId = request.Body.Id;
        
        var user = await _context.Users
            .Include(u => u.FavoriteWords)
            .FirstOrDefaultAsync(u => u.Id == userId);
        
        if (user == null)
            return FailureResponses.NotFound("User not found");
        
        var word = await _context.Words
            .FirstOrDefaultAsync(w => w.Id == wordId);
        
        if (word == null)
            return FailureResponses.NotFound("Word not found");
        
        var existingWord = user.FavoriteWords.FirstOrDefault(w => w.Id == wordId);
        
        if (existingWord != null) user.FavoriteWords.Remove(existingWord);
        else user.FavoriteWords.Add(word);
        
        await _context.SaveChangesAsync();
        
        return SuccessResponses.Ok();
    }
}