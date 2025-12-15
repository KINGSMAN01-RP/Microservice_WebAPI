using CustomerService_WebAPI.Data;
using CustomerService_WebAPI.Interfaces;
using System;

namespace CustomerService_WebAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public ICustomerRepository Customers { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Customers = new CustomerRepository(_context);
        }

        public Task<int> CompleteAsync() => _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }
}
