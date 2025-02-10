using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yape.Transactions.Domain.Entities;
using Yape.Transactions.Domain.Interfaces.Repository;
using Yape.Transactions.Infrastructure.Context;

namespace Yape.Transactions.Infrastructure.Repositories
{
    public class AccountRepository: IAccountRepository
    {

        private readonly IContextDatabase _context;
        public AccountRepository(IContextDatabase contextDatabase)
        {
            this._context = contextDatabase;

        }
        public async Task<Accounts> FindByIdAsync(Guid Id)
        {
            return await _context.Accounts.AsNoTracking()
                .FirstOrDefaultAsync(x => x.AccountId == Id);
        }
    }
}
