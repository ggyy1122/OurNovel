using OurNovel.Models;
using OurNovel.Models.Dto;
using OurNovel.Repositories;

public interface INovelRepository : IRepository<Novel, int>
{
    Task<List<CollectRankingDto>> GetTopCollectedNovelsAsync(int topN);
}
