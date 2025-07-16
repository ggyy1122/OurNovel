using OurNovel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IRecommendService
{
    Task AddAsync(int novelId, int readerId, string? reason);
    Task DeleteAsync(int novelId, int readerId);
    Task<IEnumerable<Recommend>> GetByReaderIdAsync(int readerId);
    Task<IEnumerable<Recommend>> GetByNovelIdAsync(int novelId);
    Task<IEnumerable<Recommend>> GetAllAsync();
    Task UpdateRecommendCountAsync(int novelId);
}