using OurNovel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IRateService
{
    Task AddAsync(int novelId, int readerId, int score);
    Task DeleteAsync(int novelId, int readerId);
    Task<IEnumerable<Rate>> GetByNovelIdAsync(int novelId);
    Task<IEnumerable<Rate>> GetByReaderIdAsync(int readerId);
    Task<IEnumerable<Rate>> GetAllAsync();
}
