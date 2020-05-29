using System.Linq;
using System.Collections.Generic;
using EcpayPaymentGenerator.Models;

namespace EcpayPaymentGenerator.Extensions
{
    public static class EcpayExtension
    {
        public static string ConvertToItemName(this IEnumerable<Item> items)
        {
            var itemNames = items.Select(i => $"{i.Name} {i.Price} 新台幣 x {i.Quantity}").ToList();
            return string.Join("#", itemNames);
        }

        public static int TotalAmount(this IEnumerable<Item> items)
        {
            var amount = default(int);

            foreach (var item in items)
            {
                amount += item.Price * item.Quantity;
            }

            return amount;
        }
    }
}