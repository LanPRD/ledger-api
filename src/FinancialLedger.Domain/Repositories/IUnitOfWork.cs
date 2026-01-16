namespace FinancialLedger.Domain.Repositories;

public interface IUnitOfWork {
  public Task Commit();
  public Task Flush();
  public Task ExecuteInTransaction(Func<Task> action);
}
