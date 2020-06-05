using System;
using System.Collections.Generic;
using FluentEcpay.Enums;
using FluentEcpay.Models;

namespace FluentEcpay.Interfaces
{
    public interface IPaymentTransactionConfiguration
    {
        IPaymentConfiguration WithCustomFields(object field1, object field2, object field3, object field4);
        IPaymentConfiguration New(string no, string description, DateTime date, string remark = null);
        IPaymentConfiguration UseMethod(PaymentMethod method, PaymentSubMethod? sub = null, PaymentIgnoreMethod? ignore = null);
        IPaymentConfiguration WithItems(List<Item> items, string url = null);
    }
}