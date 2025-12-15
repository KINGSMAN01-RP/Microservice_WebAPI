namespace CustomerService_WebAPI.DTOs.CustomerDTOs
{
    public class CustomerUpdateDTO
    {
        // Id is usually passed in the route, but included here for clarity
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
