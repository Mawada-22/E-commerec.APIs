namespace Domain.Entities.Idenetity
{
    public class Address
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country {  get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public String UserId { get; set; }
        public User User { get; set; }
    }
}