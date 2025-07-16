using OurNovel.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class RecommendService : IRecommendService
{
    private readonly IRecommendRepository _repository;

    public RecommendService(IRecommendRepository repository)
    {
        _repository = repository;
    }

    public async Task AddAsync(int novelId, int readerId, string? reason)
    {
        var entity = new Recommend
        {
            NovelId = novelId,
            ReaderId = readerId,
            Reason = reason
        };
        await _repository.AddAsync(entity);
    }

    public async Task DeleteAsync(int novelId, int readerId)
    {
        await _repository.DeleteAsync(novelId, readerId);
    }

    public async Task<IEnumerable<Recommend>> GetByReaderIdAsync(int readerId)
    {
        return await _repository.GetByReaderIdAsync(readerId);
    }

    public async Task<IEnumerable<Recommend>> GetByNovelIdAsync(int novelId)
    {
        return await _repository.GetByNovelIdAsync(novelId);
    }

    public async Task<IEnumerable<Recommend>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }
}