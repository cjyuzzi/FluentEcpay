using System;
using System.Collections.Generic;
using FluentEcpay.Enums;
using FluentEcpay.Models;

namespace FluentEcpay.Interfaces
{
    public interface IPaymentTransactionConfiguration
    {
        IPaymentConfiguration WithCustomFields(string field1 = null, string field2 = null, string field3 = null, string field4 = null);
        IPaymentConfiguration New(string no, string description, DateTime? date, string remark = null);
        IPaymentConfiguration UseMethod(EPaymentMethod? method, EPaymentSubMethod? sub = null, EPaymentIgnoreMethod? ignore = null);
        IPaymentConfiguration WithItems(IEnumerable<Item> items, string url = null);
    }
}