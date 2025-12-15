using CustomerService_WebAPI.Models.Customer;

namespace CustomerService_WebAPI.Interfaces
{
    public interface ICustomerRepository : IRepository<Customers>
    {
        // Add Customer-specific methods here (e.g., GetByEmail, GetCustomersWithOpenTickets)
        Task<Customers?> GetByEmailAsync(string email);
    }

}
