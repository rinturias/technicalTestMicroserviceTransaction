using Microsoft.EntityFrameworkCore;
using Yape.Transactions.Domain.Entities;

namespace Yape.Transactions.Infrastructure.Context
{
    public class ContextDatabase : DbContext, IContextDatabase
    {
        public ContextDatabase(DbContextOptions<ContextDatabase> options) : base(options)
        {

        }

        public DbSet<Transactio> Transactions { get; set; }
        public DbSet<Accounts> Accounts { get; set; }
        public DbContext GetInstance() => this;
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
