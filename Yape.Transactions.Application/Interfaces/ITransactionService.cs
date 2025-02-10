using Yape.Transactions.Application.DTO;
using Yape.Transactions.Application.DTO.Messaging;

namespace Yape.Transactions.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<ResultService> CreateTransactionAsync(TransactionCreateDto request);
    }
}
