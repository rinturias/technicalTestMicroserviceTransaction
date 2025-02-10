using Yape.Transactions.Domain.Entities;

namespace Yape.Transactions.Domain.Interfaces.Repository
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transactio Ent);
        Task<Transactio> FindByIdAsync(Guid Id);
        Task UpdateAsync(Transactio Ent);
    }
}
