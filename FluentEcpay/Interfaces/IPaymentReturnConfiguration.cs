namespace FluentEcpay.Interfaces
{
    /// <summary>
    /// 集中設定於綠界付款完成後與回應有關之參數。
    /// </summary>
    public interface IPaymentReturnConfiguration
    {
        /// <summary>
        /// 設定付款完成通知回傳網址。
        /// </summary>
        IPaymentConfiguration ToServer(string url);
        /// <summary>
        /// 設定返回特店的按鈕連結，可選擇是否需要額外的付款資訊。
        /// </summary>
        IPaymentConfiguration ToClient(string url, bool needExtraPaidInfo = false);
    }
}