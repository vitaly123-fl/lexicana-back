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
        
        var user = await _context.Users
            .Include(x=>x.FavoriteWords)
            .FirstOrDefaultAsync(x=>x.Id == userId);

        if (user is null)
            return FailureResponses.NotFound<List<WordModel>>("User not found");

        var words = user.FavoriteWords
            .Where(x => x.Language == user.Language)
            .Select(x => new WordModel()
            {
                Id = x.Id,
                Word = x.Value,
                Translation = x.Translation
            }).ToList();

        return SuccessResponses.Ok(words);
    }
}