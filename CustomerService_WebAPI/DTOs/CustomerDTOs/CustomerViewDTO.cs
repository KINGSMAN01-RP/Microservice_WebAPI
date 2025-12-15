namespace CustomerService_WebAPI.DTOs.CustomerDTOs
{
    public class CustomerViewDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public DateTime LastModified { get; set; }
    }
}
