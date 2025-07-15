using OurNovel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IRateRepository
{
    Task AddOrUpdateAsync(Rate rate);
    Task DeleteAsync(int novelId, int readerId);
    Task<IEnumerable<Rate>> GetByNovelIdAsync(int novelId);
    Task<IEnumerable<Rate>> GetByReaderIdAsync(int readerId);
    Task<IEnumerable<Rate>> GetAllAsync();
}