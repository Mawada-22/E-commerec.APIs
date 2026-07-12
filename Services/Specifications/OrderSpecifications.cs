using Domain.Contarcts;
using Domain.Entities;

namespace Services.Specifications
{
    public class OrderSpecifications : SpeceficationsAbstracut<Order>
    {
        //orders for a given buyer, most recent first
        public OrderSpecifications(string buyerEmail) : base(o => o.BuyerEmail == buyerEmail)
        {
            AddIncludes(o => o.Items);
            AddIncludes(o => o.DeliveryMethod);
            setOrderbyDescending(o => o.OrderDate);
        }

        //a single order, scoped to its buyer so one user can't fetch another user's order by guessing the id
        public OrderSpecifications(int id, string buyerEmail) : base(o => o.Id == id && o.BuyerEmail == buyerEmail)
        {
            AddIncludes(o => o.Items);
            AddIncludes(o => o.DeliveryMethod);
        }
    }
}
