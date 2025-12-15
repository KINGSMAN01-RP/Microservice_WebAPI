namespace CustomerService_WebAPI.Models.Customer
{
    public class Customers
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
