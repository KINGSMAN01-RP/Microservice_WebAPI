using CustomerService_WebAPI.DTOs.CustomerDTOs;
using CustomerService_WebAPI.Hubs;
using CustomerService_WebAPI.Interfaces;
using CustomerService_WebAPI.Models; // Assuming Models are here
using CustomerService_WebAPI.Models.Customer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;

namespace CustomerService_WebAPI.Controllers.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<CustomerHub> _hubContext; // For SignalR

        public CustomersController(IUnitOfWork unitOfWork, IHubContext<CustomerHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        // Helper method for manual mapping (Model to View DTO)
        private CustomerViewDTO MapToViewDto(Customers customer)
        {
            return new CustomerViewDTO
            {
                Id = customer.Id,
                FullName = $"{customer.FirstName} {customer.LastName}",
                EmailAddress = customer.Email,
                LastModified = customer.DateModified ?? customer.DateCreated
            };
        }

        // 1. New Customer Creation (POST)
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerCreateDTO customerDto)
        {
            // Manual Mapping: DTO to Model
            var customer = new Customers
            {
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                Email = customerDto.Email,
                DateCreated = DateTime.UtcNow,
                DateModified = null
            };

            _unitOfWork.Customers.Add(customer);
            await _unitOfWork.CompleteAsync(); // Save to DB

            // Manual Mapping: Model to View DTO
            var viewDto = MapToViewDto(customer);

            // REAL-TIME: Notify all connected clients of the new customer
            await _hubContext.Clients.All.SendAsync("ReceiveNewCustomer", viewDto);

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, viewDto);
        }

        // 2. View Customer (GET by Id)
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerViewDTO>> GetCustomer(int id)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id);
            if (customer == null) return NotFound();

            // Manual Mapping: Model to View DTO
            return Ok(MapToViewDto(customer));
        }

        // 3. View Customers in List (GET All)
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CustomerViewDTO>>> GetCustomers()
        {
            var customers = await _unitOfWork.Customers.GetAllAsync();

            // Manual Mapping: IEnumerable<Model> to IEnumerable<View DTO>
            var viewDtos = customers.Select(c => MapToViewDto(c)).ToList();

            return Ok(viewDtos);
        }

        // 4. Update Customer (PUT)
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerUpdateDTO customerDto)
        {
            if (id != customerDto.Id) return BadRequest("ID mismatch");

            var customerToUpdate = await _unitOfWork.Customers.GetByIdAsync(id);
            if (customerToUpdate == null) return NotFound();

            // Manual Mapping: Update DTO properties onto the existing entity
            customerToUpdate.FirstName = customerDto.FirstName;
            customerToUpdate.LastName = customerDto.LastName;
            customerToUpdate.Email = customerDto.Email;
            customerToUpdate.DateModified = DateTime.UtcNow;

            _unitOfWork.Customers.Update(customerToUpdate);
            await _unitOfWork.CompleteAsync();

            // Manual Mapping: Model to View DTO
            var viewDto = MapToViewDto(customerToUpdate);

            // REAL-TIME: Notify clients of the update
            await _hubContext.Clients.All.SendAsync("ReceiveCustomerUpdate", viewDto);

            return NoContent();
        }

        // 5. Delete Customer (DELETE)
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customerToDelete = await _unitOfWork.Customers.GetByIdAsync(id);
            if (customerToDelete == null) return NotFound();

            _unitOfWork.Customers.Delete(customerToDelete);
            await _unitOfWork.CompleteAsync();

            // REAL-TIME: Notify clients of the deletion (sending the deleted ID)
            await _hubContext.Clients.All.SendAsync("ReceiveCustomerDelete", id);

            return NoContent();
        }
    }
}