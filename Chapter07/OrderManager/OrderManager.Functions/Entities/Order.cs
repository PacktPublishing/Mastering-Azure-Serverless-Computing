using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace OrderManager.Core.Entities
{
    public class Order:TableEntity
    {
        public Order()
        {

        }

        public Order(string customer,string orderId) : base(customer, orderId)
        {
            this.Id=orderId;
        }

        public string Id { get; set; }

        public string Customer { get; set; }
        public string CustomerMail { get; set; }

        public decimal Amount { get; set; }
        public DateTimeOffset CreationTimestamp { get; set; }
        public OrderStatus State { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}={Id}, {nameof(Customer)}={Customer}, {nameof(CustomerMail)}={CustomerMail}, {nameof(Id)}={Id}, {nameof(Amount)}={Amount},  {nameof(CreationTimestamp)}={CreationTimestamp},  {nameof(State)}={State}";
        }
    }
}
