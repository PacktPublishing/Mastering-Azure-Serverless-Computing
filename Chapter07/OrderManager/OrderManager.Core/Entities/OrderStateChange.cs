using System;
using System.Collections.Generic;
using System.Text;

namespace OrderManager.Core.Entities
{
    public class OrderStateChange
    {
        public string OrderId { get; set; }
        public OrderStatus NewOrderState { get; set; }
    }
}
