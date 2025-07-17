using OurNovel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICollectService
{
    Task AddAsync(int novelId, int readerId, string? isPublic);
    Task AddOrUpdateAsync(int novelId, int readerId, string? isPublic);
    Task DeleteAsync(int novelId, int readerId);
    Task<IEnumerable<Collect>> GetByReaderIdAsync(int readerId);
    Task<IEnumerable<Collect>> GetByNovelIdAsync(int novelId);
    Task<IEnumerable<Collect>> GetAllAsync();
    Task UpdateCollectCountAsync(int novelId);
}
