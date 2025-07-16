using OurNovel.Models;
using System.Threading.Tasks;

public interface IRateRepository
{
    Task AddAsync(Rate entity);
    Task DeleteAsync(int novelId, int readerId);
    Task<Rate?> FindAsync(int novelId, int readerId);  // 新增
    Task<IEnumerable<Rate>> GetByNovelIdAsync(int novelId);
    Task<IEnumerable<Rate>> GetByReaderIdAsync(int readerId);
    Task<IEnumerable<Rate>> GetAllAsync();
}
