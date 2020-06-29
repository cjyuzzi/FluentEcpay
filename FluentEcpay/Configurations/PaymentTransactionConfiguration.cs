using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using FluentEcpay.Interfaces;

namespace FluentEcpay.Configurations
{
    public class PaymentTransactionConfiguration : IPaymentTransactionConfiguration
    {
        #region Private Fields
        private readonly PaymentConfiguration _configuration;
        private readonly Action<IPayment> _setPayment;
        private IPayment _payment;
        private Random random = new Random();
        #endregion

        #region CTOR
        public PaymentTransactionConfiguration(PaymentConfiguration configuration, Action<IPayment> setPayment)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _setPayment = setPayment ?? throw new ArgumentNullException(nameof(setPayment));
        }
        #endregion

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
                _payment.Remark = remark;

            _setPayment(_payment);

            return _configuration;
        }

        public IPaymentConfiguration UseMethod(EPaymentMethod? method, EPaymentSubMethod? sub = null, EPaymentIgnoreMethod? ignore = null)
        {
            if (!method.HasValue) throw new ArgumentNullException(nameof(method));

            switch (method.Value)
            {
                case EPaymentMethod.ALL:
                    if (sub.HasValue) throw new ArgumentException("Sub method is not allowed when PaymentMethod is set to ALL.", nameof(sub));
                    _payment.ChoosePayment = method.Value.ToString();
                    if (ignore.HasValue)
                        _payment.IgnorePayment = ignore.Value.ToString("F").Replace(", ", "#");
                    break;
                case EPaymentMethod.Union:
                    if (sub.HasValue) throw new ArgumentException("Sub method is not allowed when PaymentMethod is set to Union.", nameof(sub));
                    if (ignore.HasValue) throw new ArgumentException("Ignore method is not allowed when PaymentMethod is not set to ALL.", nameof(ignore));
                    _payment.ChoosePayment = EPaymentMethod.Credit.ToString();
                    _payment.UnionPay = 1;
                    break;
                case EPaymentMethod.ATM:
                case EPaymentMethod.CVS:
                case EPaymentMethod.BARCODE:
                case EPaymentMethod.Credit:
                case EPaymentMethod.WebATM:
                    if (ignore.HasValue) throw new ArgumentException("Ignore method is not allowed when PaymentMethod is not set to ALL.", nameof(ignore));
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

        public IPaymentConfiguration WithItems(IEnumerable<IItem> items, string url = null, int? amount = null)
        {
            if (items is null || items.Count() == 0) throw new ArgumentNullException(nameof(items));

            _payment.TotalAmount = amount.HasValue ? amount : CalculateTotalAmount(items);
            _payment.ItemName = GenerateItemName(items);

            if (url != null) _payment.ItemURL = url;

            return _configuration;
        }

        #region Private Methods
        private string GenerateTradeNo(string no)
        {
            var randomLength = 20 - no.Length;
            return no + GenerateRandomNo(randomLength);
        }
        private string GenerateRandomNo(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private int CalculateTotalAmount(IEnumerable<IItem> items)
        {
            var amount = 0;

            foreach (var item in items)
            {
                amount += item.Price * item.Quantity;
            }

            return amount;
        }
        private string GenerateItemName(IEnumerable<IItem> items)
        {
            var itemNames = items.Select(i => $"{i.Name} {i.Price} 新臺幣 x {i.Quantity}");
            return string.Join("#", itemNames);
        }
        #endregion
    }
}