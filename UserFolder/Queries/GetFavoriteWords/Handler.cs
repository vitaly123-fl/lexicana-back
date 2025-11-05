using MediatR;
using lexicana.Database;
using lexicana.Endpoints;
using Microsoft.EntityFrameworkCore;
using lexicana.Authorization.Services;
using lexicana.TopicFolder.WordFolder.DTOs;

namespace lexicana.UserFolder.Queries.GetFavoriteWords;

public record GetFavoriteWordsRequest(): IHttpRequest<List<WordModel>>;

public class Handler : IRequestHandler<GetFavoriteWordsRequest, Response<List<WordModel>>>
{
    private readonly AuthService _authService;
    private readonly ApplicationDbContext _context;
    
    public Handler(ApplicationDbContext context, AuthService authService)
    {
        _context = context;
        _authService = authService;
    }
    
    public async Task<Response<List<WordModel>>> Handle(GetFavoriteWordsRequest request, CancellationToken cancellationToken)
    {
        var userId = _authService.GetCurrentUserId();

        var userLanguage = await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => u.Language)
            .FirstOrDefaultAsync(cancellationToken);

        if (userLanguage is null)
            return FailureResponses.NotFound<List<WordModel>>("User not found");

        var favoriteWords = await _context.Words
            .Where(w => w.UsersForFavorite.Any(u => u.Id == userId) && w.Language == userLanguage)
            .Select(w => new WordModel
            {
                Id = w.Id,
                Word = w.Value,
                Translation = w.Translation
            })
            .ToListAsync(cancellationToken);

        return SuccessResponses.Ok(favoriteWords);
    }
}