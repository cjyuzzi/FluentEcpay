namespace FluentEcpay.Interfaces
{
    /// <summary>
    /// 產生綠界金流訂單的主要設定檔。
    /// </summary>
    public interface IPaymentConfiguration
    {
        /// <summary>
        /// 集中設定與綠界金流服務介接有關之參數。
        /// </summary>
        IPaymentSendConfiguration Send { get; }
        /// <summary>
        /// 集中設定於綠界付款完成後與回應有關之參數。
        /// </summary>
        IPaymentReturnConfiguration Return { get; }
        /// <summary>
        /// 集中設定與綠界訂單交易有關之參數。
        /// </summary>
        IPaymentTransactionConfiguration Transaction { get; }
        /// <summary>
        /// 產生綠界訂單之物件實體。
        /// </summary>
        IPayment Generate();
    }
}