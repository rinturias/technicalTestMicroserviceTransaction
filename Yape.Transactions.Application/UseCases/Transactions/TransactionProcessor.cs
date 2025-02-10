using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yape.Transactions.Application.DTO.Messaging;
using Yape.Transactions.Application.DTO;
using Yape.Transactions.Application.Exeptions;
using Yape.Transactions.Domain.Interfaces;
using Yape.Transactions.Domain.Interfaces.Repository;
using Confluent.Kafka;
using Yape.Transactions.Domain.Entities;

namespace Yape.Transactions.Application.UseCases.Transactions
{
    public class TransactionProcessor : ITransactionProcessor
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProducer<string, string> _producer;
        public TransactionProcessor(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork, IProducer<string, string> producer)
        {
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
            _producer = producer;

        }
        public async Task<bool> UpdateTransactionStatusAsync(dynamic data)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                Transactio dataTransaction = await _transactionRepository.FindByIdAsync(data.TransactionId);
                if (dataTransaction == null)
                    throw new Exception($"Transaction with ID {data.TransactionId} not found.");
                dataTransaction.CreatedAt = dataTransaction.CreatedAt.ToUniversalTime();
                dataTransaction.Status= data.Status;
                 var resultUpdate = _transactionRepository.UpdateAsync(dataTransaction);

                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {

                _unitOfWork.Rollback();return false;
            }

        }

      
    }
}
