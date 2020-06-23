using System;
using System.Collections.Generic;

namespace FluentEcpay.Interfaces
{
    /// <summary>
    /// 集中設定與綠界訂單交易有關之參數。
    /// </summary>
    public interface IPaymentTransactionConfiguration
    {
        /// <summary>
        /// 建立新的綠界交易訂單，需要傳入訂單編號、描述以及日期。
        /// </summary>
        IPaymentConfiguration New(string no, string description, DateTime? date, string remark = null);
        /// <summary>
        /// 指定綠界訂單交易所使用的付款方式。
        /// </summary>
        IPaymentConfiguration UseMethod(EPaymentMethod? method, EPaymentSubMethod? sub = null, EPaymentIgnoreMethod? ignore = null);
        /// <summary>
        /// 設定綠界交易訂單的商品項目。
        /// </summary>
        IPaymentConfiguration WithItems(IEnumerable<IItem> items, string url = null, int? amount = null);
        /// <summary>
        /// 提供合作廠商使用記錄用客製化使用欄位。
        /// </summary>
        IPaymentConfiguration WithCustomFields(string field1 = null, string field2 = null, string field3 = null, string field4 = null);
    }
}