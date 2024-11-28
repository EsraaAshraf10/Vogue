
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VogueCore.Entities.order_Aggregate;

namespace VogueCore.Specifications.Order_Spec
{
    public class OrderSpecifications:BaseSpecifications<Order>
    {
        public OrderSpecifications()
        {
            Includes.Add(O => O.Items);
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.ShippingAddress);
            AddOrderByDescending(O => O.OrderDate);
        }
        public OrderSpecifications(string email):base(O=>O.BuyerEmail == email)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
            AddOrderByDescending(O=>O.OrderDate);
        }
        public OrderSpecifications(string email, int OrderId) : base(O=>O.BuyerEmail == email && O.Id == OrderId)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }
    } 
}
