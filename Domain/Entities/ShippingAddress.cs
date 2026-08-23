namespace Domain.Entities
{
    //owned type on Order - not an aggregate root, no Id/ModelBase
    public class ShippingAddress
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
