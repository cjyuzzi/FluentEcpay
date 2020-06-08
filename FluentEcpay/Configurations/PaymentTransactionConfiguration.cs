using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using FluentEcpay.Interfaces;
using FluentEcpay.Models;
using FluentEcpay.Enums;

namespace FluentEcpay.Configurations
{
    public class PaymentTransactionConfiguration : IPaymentTransactionConfiguration
    {
        private readonly PaymentConfiguration _configuration;
        private readonly Action<IPayment> _setPayment;
        private IPayment _payment;

        public PaymentTransactionConfiguration(PaymentConfiguration paymentConfiguration, Action<IPayment> setPayment)
        {
            _configuration = paymentConfiguration ?? throw new ArgumentNullException(nameof(paymentConfiguration));
            _setPayment = setPayment ?? throw new ArgumentNullException(nameof(setPayment));
        }

        public IPaymentConfiguration New(string no, string description, DateTime? date, string remark = null)
        {
            if (string.IsNullOrEmpty(no)) throw new ArgumentNullException(nameof(no));
            if (string.IsNullOrEmpty(description)) throw new ArgumentNullException(nameof(description));
            if (!date.HasValue) throw new ArgumentNullException(nameof(date));

            _payment = new Payment
            {
                MerchantTradeNo = GenerateTradeNo(no),
                TradeDesc = HttpUtility.UrlEncode(description),
                MerchantTradeDate = date.Value.ToString("yyyy/MM/dd HH:mm:ss")
            };
            if (remark != null)
                _payment.Remark = HttpUtility.UrlEncode(remark);

            _setPayment(_payment);

            return _configuration;
        }

        public IPaymentConfiguration UseMethod(PaymentMethod? method, PaymentSubMethod? sub = null, PaymentIgnoreMethod? ignore = null)
        {
            if (!method.HasValue) throw new ArgumentNullException(nameof(method));

            switch (method.Value)
            {
                case PaymentMethod.ALL:
                    if (!ignore.HasValue) throw new ArgumentNullException(nameof(ignore));
                    if (sub.HasValue) throw new ArgumentException("Sub Method is not allowed when PaymentMethod is set to ALL.", nameof(sub));
                    _payment.ChoosePayment = method.Value.ToString();
                    _payment.IgnorePayment = ignore.Value.ToString("F").Replace(", ", "#");
                    break;
                case PaymentMethod.Union:
                    if (sub.HasValue) throw new ArgumentException("Sub Method is not allowed when PaymentMethod is set to Union.", nameof(sub));
                    if (ignore.HasValue) throw new ArgumentException("Ignore Method is not allowed when PaymentMethod is not set to ALL.", nameof(ignore));
                    _payment.ChoosePayment = PaymentMethod.Credit.ToString();
                    _payment.UnionPay = 1;
                    break;
                case PaymentMethod.ATM:
                case PaymentMethod.CVS:
                case PaymentMethod.BARCODE:
                case PaymentMethod.Credit:
                case PaymentMethod.WebATM:
                    if (ignore.HasValue) throw new ArgumentException("Ignore Method is not allowed when PaymentMethod is not set to ALL.", nameof(ignore));
                    _payment.ChoosePayment = method.Value.ToString();
                    if (sub.HasValue) _payment.ChooseSubPayment = sub.Value.ToString();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(method));
            }

            return _configuration;
        }

        public IPaymentConfiguration WithCustomFields(string field1 = null, string field2 = null, string field3 = null, string field4 = null)
        {
            if (field1 != null) _payment.CustomField1 = field1;
            if (field2 != null) _payment.CustomField2 = field2;
            if (field3 != null) _payment.CustomField3 = field3;
            if (field4 != null) _payment.CustomField4 = field4;

            return _configuration;
        }

        public IPaymentConfiguration WithItems(IEnumerable<Item> items, string url = null)
        {
            if (items is null || items.Count() == 0) throw new ArgumentNullException(nameof(items));

            _payment.ItemName = GenerateItemName(items);
            _payment.TotalAmount = CalculateTotalAmount(items);

            return _configuration;
        }


        #region private methods
        private string GenerateTradeNo(string no)
        {
            var randomLength = 20 - no.Length;
            var random = new Random().Next(0, int.MaxValue);

            return no + random.ToString().PadLeft(randomLength, '0');
        }
        private int CalculateTotalAmount(IEnumerable<Item> items)
        {
            var amount = 0;

            foreach (var item in items)
            {
                amount += item.Price * item.Quantity;
            }

            return amount;
        }
        private string GenerateItemName(IEnumerable<Item> items)
        {
            var itemNames = items.Select(i => $"{i.Name} {i.Price} 新臺幣 x {i.Quantity}").ToList();
            return string.Join("#", itemNames);
        }
        #endregion
    }
}