using Microsoft.EntityFrameworkCore.Storage;
using Yape.Transactions.Domain.Interfaces.Repository;
using Yape.Transactions.Infrastructure.Context;


namespace Yape.Transactions.Infrastructure.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IContextDatabase _context;
        private IDbContextTransaction _transaction;

        public UnitOfWork(IContextDatabase context)
        {
            _context = context;
        }

        public IContextDatabase Context => _context;

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Commit()
        {
            try
            {
                _context.SaveChanges();
                _transaction?.Commit();
            }
            catch
            {
                Rollback();
                throw;
            }
        }

        public void Rollback()
        {
            _transaction?.Rollback();
        }


    }
}
