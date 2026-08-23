namespace Domain.Entities
{
    public class OrderItem : ModelBase<int>
    {
        public ProductItemOrdered Product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
