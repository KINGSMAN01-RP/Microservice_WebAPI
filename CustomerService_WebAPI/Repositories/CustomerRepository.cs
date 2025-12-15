using CustomerService_WebAPI.Data;
using CustomerService_WebAPI.Interfaces;
using CustomerService_WebAPI.Models.Customer;
using Microsoft.EntityFrameworkCore;
using System;

namespace CustomerService_WebAPI.Repositories
{  
    public class CustomerRepository : Repository<Customers>, ICustomerRepository
    {
        public CustomerRepository(AppDbContext context) : base(context) { }

        public async Task<Customers?> GetByEmailAsync(string email) =>
            await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
    }

}
