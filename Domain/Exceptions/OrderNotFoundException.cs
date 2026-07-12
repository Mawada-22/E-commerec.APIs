namespace Domain.Exceptions
{
    public sealed class OrderNotFoundException : NotFoundException
    {
        public OrderNotFoundException(int id) : base($"Order With Id :{id} is not found")
        {
        }
    }
}
