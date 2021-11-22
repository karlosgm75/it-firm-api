namespace it_firm_api.Domain.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string FistName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
    }
}
