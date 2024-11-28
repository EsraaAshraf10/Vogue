using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VogueCore.Entities.order_Aggregate;

namespace VogueCore.Specifications.Order_Spec
{
    public class OrderWithPaymentIntentSpec : BaseSpecifications<Order>
    {
        public OrderWithPaymentIntentSpec(string PaymentIntentId):base(O=>O.PaymentIntentId == PaymentIntentId)
        {
            
        }
    }
}
