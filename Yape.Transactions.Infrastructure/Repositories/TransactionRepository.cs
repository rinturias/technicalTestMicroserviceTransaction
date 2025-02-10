using Microsoft.EntityFrameworkCore;
using Yape.Transactions.Domain.Entities;
using Yape.Transactions.Domain.Interfaces.Repository;
using Yape.Transactions.Infrastructure.Context;

namespace Yape.Transactions.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IContextDatabase _context;
        public TransactionRepository(IContextDatabase contextDatabase)
        {
            this._context = contextDatabase;

        }
        public async Task AddAsync(Transactio Ent)
        {

            await _context.Transactions.AddAsync(Ent);

        }
        public async Task<Transactio> FindByIdAsync(Guid Id)
        {
            return await _context.Transactions.AsNoTracking()
                .FirstOrDefaultAsync(x => x.TransactionId == Id);
        }
        public Task UpdateAsync(Transactio Ent)
        {
            _context.Transactions.Update(Ent);
            return Task.CompletedTask;
        }
    }
}
