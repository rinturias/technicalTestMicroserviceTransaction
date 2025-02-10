namespace Yape.Transactions.Domain.Interfaces.Repository
{
    public interface IUnitOfWork
    {

        void BeginTransaction();
        void Commit();
        void Rollback();
        Task SaveChangesAsync();
    }
}
