using OurNovel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IRecommendRepository
{
    Task AddAsync(Recommend entity);
    Task DeleteAsync(int novelId, int readerId);
    Task<IEnumerable<Recommend>> GetByReaderIdAsync(int readerId);
    Task<IEnumerable<Recommend>> GetByNovelIdAsync(int novelId);
    Task<IEnumerable<Recommend>> GetAllAsync();
}