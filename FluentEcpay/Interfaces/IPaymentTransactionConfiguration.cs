using System;
using System.Collections.Generic;
using FluentEcpay.Enums;
using FluentEcpay.Models;

namespace FluentEcpay.Interfaces
{
    public interface IPaymentTransactionConfiguration
    {
        IPaymentConfiguration New(string no, string description, DateTime date, Action<ITransactionOptionsConfiguration> configureTransaction);
        IPaymentConfiguration WithItems(List<Item> items, Action<IItemOptionsConfiguration> configureItems);
        IPaymentConfiguration UseMethod(PaymentMethod method, Action<IMethodOptionsConfiguration> configureMethod);
    }
}