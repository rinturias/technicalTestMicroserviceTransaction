using Confluent.Kafka;
using System.Text.Json;
using System.Transactions;
using Yape.Transactions.Application.DTO;
using Yape.Transactions.Application.DTO.Messaging;
using Yape.Transactions.Application.Exeptions;
using Yape.Transactions.Application.Interfaces;
using Yape.Transactions.Domain.Entities;
using Yape.Transactions.Domain.Enums;
using Yape.Transactions.Domain.Interfaces.Repository;
using Yape.Transactions.Utils.Exceptions;
using TransactionStatus = Yape.Transactions.Domain.Enums.TransactionStatus;

namespace Yape.Transactions.Application.UseCases.Transactions
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProducer<string, string> _producer;
        public TransactionService(ITransactionRepository transactionRepository, IAccountRepository accountRepository, IUnitOfWork unitOfWork, IProducer<string, string> producer)
        {
            _transactionRepository = transactionRepository;
            _accountRepository= accountRepository;
            _unitOfWork = unitOfWork;
            _producer = producer;

        }

        public async Task<ResultService> CreateTransactionAsync(TransactionCreateDto request)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var dataAccount =await _accountRepository.FindByIdAsync(request.SourceAccountId);
                if(dataAccount == null) throw new ApiErrorDataException(CodeExceptions.COD204, MessageErrorTransaction.COD0005);

                var objEntity = new Transactio
                {
                    TransactionId = Guid.NewGuid(),  
                    SourceAccountId = request.SourceAccountId, 
                    TargetAccountId = request.TargetAccountId,  
                    TransferTypeId = request.TransferTypeId,  
                    Amount = request.Value, 
                    Status = TransactionStatus.Pending,  
                    CreatedAt = DateTime.UtcNow,  
                };
                await _transactionRepository.AddAsync(objEntity);


                var message = JsonSerializer.Serialize(objEntity);
                _producer.Produce("transactions", new Message<string, string> { Key = objEntity.TransactionId.ToString(), Value = message });

                _unitOfWork.Commit();
                return new ResultService { Success = true, CodError = CodeExceptions.COD200, Messaje = "Ok.", Data = objEntity};
            }
            catch (ApiErrorDataException ex)
            {
                return HandleException(ex.ErrorCode, ex.Message);
            }
            catch (Exception ex)
            {
                return HandleException(CodeExceptions.COD422, "NotOk", ex.Message);
            }
        }

      

        private ResultService HandleException(string errorCode, string message, string error = null)
        {

            _unitOfWork.Rollback();
            return new ResultService
            {
                Success = false,
                CodError = errorCode,
                Messaje = message,
                Error = error
            };
        }


    }
}
