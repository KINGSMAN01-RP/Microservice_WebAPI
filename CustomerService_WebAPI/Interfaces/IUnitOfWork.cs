namespace CustomerService_WebAPI.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository Customers { get; }
        Task<int> CompleteAsync(); // Saves all changes in the context
    }
}
