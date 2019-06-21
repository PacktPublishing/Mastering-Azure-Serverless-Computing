using System;

namespace OrderManager.Core.Rest
{
    public class OrderDto
    {
        public string Customer { get; set; }
        public string CustomerMail { get; set; }

        public decimal Amount { get; set; }
        public DateTimeOffset CreationTimestamp { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Customer) &&
                   !string.IsNullOrWhiteSpace(CustomerMail) &&
                   Amount >= 0;
        }
    }
}
