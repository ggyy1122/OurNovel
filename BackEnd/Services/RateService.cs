using OurNovel.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class RateService : IRateService
{
    private readonly IRateRepository _repository;

    public RateService(IRateRepository repository)
    {
        _repository = repository;
    }

    public async Task AddOrUpdateAsync(int novelId, int readerId, int score)
    {
        var rate = new Rate
        {
            NovelId = novelId,
            ReaderId = readerId,
            Score = score,
            RatingTime = DateTime.UtcNow
        };
        await _repository.AddOrUpdateAsync(rate);
    }

    public async Task DeleteAsync(int novelId, int readerId)
    {
        await _repository.DeleteAsync(novelId, readerId);
    }

    public async Task<IEnumerable<Rate>> GetByNovelIdAsync(int novelId)
    {
        return await _repository.GetByNovelIdAsync(novelId);
    }

    public async Task<IEnumerable<Rate>> GetByReaderIdAsync(int readerId)
    {
        return await _repository.GetByReaderIdAsync(readerId);
    }

    public async Task<IEnumerable<Rate>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }
}