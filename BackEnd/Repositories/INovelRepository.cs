using System.Linq.Expressions;
using OurNovel.Models;
using OurNovel.Models.Dto;
using OurNovel.Repositories;

public interface INovelRepository : IRepository<Novel, int>
{
    Task<List<CollectRankingDto>> GetTopCollectedNovelsAsync(int topN, string? status = null);
    Task<List<RecommendRankingDto>> GetTopRecommendedNovelsAsync(int topN, string? status = null);
    Task<List<ScoreRankingDto>> GetTopScoredNovelsAsync(int topN, string? status = null);

    Task<TResult> GetNovelPropertyAsync<TResult>(int novelId, Expression<Func<Novel, TResult>> propertySelector);

}
