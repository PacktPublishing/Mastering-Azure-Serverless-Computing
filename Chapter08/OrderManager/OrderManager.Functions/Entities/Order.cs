using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace OrderManager.Core.Entities
{
    public class Order : TableEntity
    {
        public Order()
        {

        }

        public Order(string customer, string orderId) : base(customer, orderId)
        {
            this.Id = orderId;

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

        public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            var results = base.WriteEntity(operationContext);

            var stateProperty = new EntityProperty(this.State.ToString());
            results.Add(nameof(State), stateProperty);

            var amountProperty = new EntityProperty(this.Amount.ToString(System.Globalization.CultureInfo.InvariantCulture));
            results.Add(nameof(Amount), amountProperty);

            return results;
        }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            base.ReadEntity(properties, operationContext);
            if (properties.ContainsKey(nameof(State)))
            {
                var property = properties[nameof(State)];
                this.State = (OrderStatus)Enum.Parse(typeof(OrderStatus), property.StringValue);
                properties.Remove(nameof(State));
            }
            if (properties.ContainsKey(nameof(Amount)))
            {
                var property = properties[nameof(Amount)];
                this.Amount = decimal.Parse(property.StringValue, System.Globalization.CultureInfo.InvariantCulture);
                properties.Remove(nameof(Amount));
            }
        }
    }
}
