using OurNovel.Models;
using OurNovel.Repositories;

namespace OurNovel.Services
{
    public class ManagementService : BaseService<Management, int>
    {
        public ManagementService(IRepository<Management, int> repository) : base(repository) { }

        public async Task<IEnumerable<Management>> GetByManagerIdAsync(int managerId)
        {
            var all = await _repository.GetAllAsync();
            return all.Where(m => m.ManagerId == managerId);
        }
    }
}
