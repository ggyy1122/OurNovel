using OurNovel.Models;
using OurNovel.Repositories;

public class AuthorIncomeService
{
    private readonly IRepository<AuthorIncome, long> _repository;

    public AuthorIncomeService(IRepository<AuthorIncome, long> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AuthorIncome>> GetByAuthorIdAsync(long authorId)
    {
        var incomes = await _repository.GetAllAsync();
        return incomes.Where(i => i.AuthorId == authorId);
    }

    public async Task<decimal> GetTotalIncomeAsync(long authorId)
    {
        var incomes = await _repository.GetAllAsync();
        return incomes
            .Where(i => i.AuthorId == authorId)
            .Sum(i => i.Amount);
    }
}
