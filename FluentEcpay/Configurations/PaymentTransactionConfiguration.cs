using System;
using System.Collections.Generic;
using FluentEcpay.Interfaces;
using FluentEcpay.Models;
using FluentEcpay.Enums;

namespace FluentEcpay.Configurations
{
    public class PaymentTransactionConfiguration : IPaymentTransactionConfiguration
    {
        private PaymentConfiguration _configuration;
        private Action<IPayment> _setPayment;
        private IPayment _payment;

        public PaymentTransactionConfiguration(PaymentConfiguration paymentConfiguration, Action<IPayment> setPayment)
        {
            _configuration = paymentConfiguration;
            _setPayment = setPayment;
        }

        public IPaymentConfiguration New(string no, string description, DateTime date, string remark = null)
        {
            throw new NotImplementedException();
        }

        public IPaymentConfiguration UseMethod(PaymentMethod method, PaymentSubMethod? sub = null, PaymentIgnoreMethod? ignore = null)
        {
            throw new NotImplementedException();
        }

        public IPaymentConfiguration WithCustomFields(object field1, object field2, object field3, object field4)
        {
            throw new NotImplementedException();
        }

        public IPaymentConfiguration WithItems(List<Item> items, string url = null)
        {
            throw new NotImplementedException();
        }
    }
}