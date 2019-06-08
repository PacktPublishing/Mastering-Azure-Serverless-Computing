using OrderManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderManager.Core
{
    public static class TextUtility
    {
        public static string GenerateMailSubject(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            switch (order.State)
            {
                case OrderStatus.Cancelled:
                    return $"Order {order.Id } cancelled";
                case OrderStatus.Paid:
                    return $"Order {order.Id } paid";
                case OrderStatus.Created:
                case OrderStatus.Error:
                default:
                    throw new NotSupportedException();
            }
        }

        public static string GenerateMailBody(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            string body = null;
            switch (order.State)
            {
                case OrderStatus.Cancelled:
                    body=$"Your order {order.Id} created on {order.CreationTimestamp} is cancelled\n\n";
                    body+=$"Customer : {order.Customer}\n\n";
                    body += $"Email: {order.CustomerMail}\n\n";
                    body += $"Amount : {order.Amount}€\n\n";
                    return body;
                case OrderStatus.Paid:
                    body = $"Your order {order.Id} created on {order.CreationTimestamp} is paid on {DateTime.Now}\n\n";
                    body += $"Customer : {order.Customer}\n\n";
                    body += $"Email: {order.CustomerMail}\n\n";
                    body += $"Amount : {order.Amount}€\n\n";
                    return body;
                case OrderStatus.Created:
                case OrderStatus.Error:
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
