namespace Domain.Entities
{
    //owned type on OrderItem - a snapshot of the product at order time, deliberately decoupled from the live Product row
    public class ProductItemOrdered
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
    }
}
