using OurNovel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICollectRepository
{
    Task AddAsync(Collect entity);
    Task AddOrUpdateAsync(Collect entity);
    Task DeleteAsync(int novelId, int readerId);
    Task<IEnumerable<Collect>> GetByReaderIdAsync(int readerId);
    Task<IEnumerable<Collect>> GetByNovelIdAsync(int novelId);
    Task<IEnumerable<Collect>> GetAllAsync();
}
