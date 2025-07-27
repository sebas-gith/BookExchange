namespace BookExchange.Application.DTOs.Students
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Campus { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}