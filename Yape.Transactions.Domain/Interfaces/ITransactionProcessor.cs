using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yape.Transactions.Domain.Interfaces
{
    public interface ITransactionProcessor
    {
        Task<bool> UpdateTransactionStatusAsync(dynamic data);
    }
}
