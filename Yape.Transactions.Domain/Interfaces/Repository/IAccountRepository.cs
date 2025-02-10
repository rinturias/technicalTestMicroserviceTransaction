using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yape.Transactions.Domain.Entities;

namespace Yape.Transactions.Domain.Interfaces.Repository
{
    public interface IAccountRepository
    {
        Task<Accounts> FindByIdAsync(Guid Id);
    }
}
