using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yape.Transactions.Infrastructure.Context;
using Yape.Transactions.Infrastructure.UnitOfWorks;

namespace Yape.Transactions.Test.Infrastructure.UnitOfWorks
{
    public class UnitOfWork_Test
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly Mock<IContextDatabase> _mockContext;
        private readonly Mock<IDbContextTransaction> _mockTransaction;

        public UnitOfWork_Test()
        {
            _mockContext = new Mock<IContextDatabase>();
            _mockTransaction = new Mock<IDbContextTransaction>();

            var mockDatabaseFacade = new Mock<DatabaseFacade>(_mockContext.Object as DbContext);
            mockDatabaseFacade.Setup(db => db.BeginTransaction()).Returns(_mockTransaction.Object);
            _mockContext.Setup(c => c.Database).Returns(mockDatabaseFacade.Object);
            _unitOfWork = new UnitOfWork(_mockContext.Object);
        }

        [Fact]
        public void BeginTransaction_CallsDatabaseBeginTransaction()
        {
           
            _unitOfWork.BeginTransaction();

          
            _mockContext.Verify(c => c.Database.BeginTransaction(), Times.Once);
        }

        [Fact]
        public async Task SaveChangesAsync_CallsContextSaveChangesAsync()
        {
          
            await _unitOfWork.SaveChangesAsync();

         
            _mockContext.Verify(c => c.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public void Commit_CallsSaveChangesAndTransactionCommit()
        {
           
            _unitOfWork.BeginTransaction();
            _unitOfWork.Commit();

       
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
            _mockTransaction.Verify(t => t.Commit(), Times.Once);
        }

        [Fact]
        public void Commit_OnException_CallsRollback()
        {
            
            _unitOfWork.BeginTransaction();
            _mockContext.Setup(c => c.SaveChanges()).Throws(new Exception());

            Assert.Throws<Exception>(() => _unitOfWork.Commit());
            _mockTransaction.Verify(t => t.Rollback(), Times.Once);
        }

        [Fact]
        public void Rollback_CallsTransactionRollback()
        {
     
            _unitOfWork.BeginTransaction();
            _unitOfWork.Rollback();
            _mockTransaction.Verify(t => t.Rollback(), Times.Once);
        }
    }
}