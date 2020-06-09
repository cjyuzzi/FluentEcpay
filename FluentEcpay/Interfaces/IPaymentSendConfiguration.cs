namespace FluentEcpay.Interfaces
{
    /// <summary>
    /// 集中設定與綠界金流服務介接有關之參數。
    /// </summary>
    public interface IPaymentSendConfiguration
    {
        /// <summary>
        /// 設定綠界金流服務之介接路徑。
        /// </summary>
        IPaymentConfiguration ToApi(string url);
        /// <summary>
        /// 設定於綠界金流註冊之特店相關參數，可選擇設定旗下的店舖代號以及是否為特約合作平台商。
        /// </summary>
        IPaymentConfiguration ToMerchant(string merchantId, string storeId = null, bool isPlatform = false);
        /// <summary>
        /// 設定介接時一併傳送的檢查碼所需的雜湊參數。
        /// </summary>
        IPaymentConfiguration UsingHash(string key, string iv, EHashAlgorithm? algorithm = EHashAlgorithm.SHA256);
    }
}