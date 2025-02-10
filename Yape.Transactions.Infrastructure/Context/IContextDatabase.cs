using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Yape.Transactions.Domain.Entities;

namespace Yape.Transactions.Infrastructure.Context
{
    public interface IContextDatabase
    {
        DbSet<Transactio> Transactions { get; set; }
        DbSet<Accounts> Accounts { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
        DatabaseFacade Database { get; }
    }
}
