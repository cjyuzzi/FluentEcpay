using System.Net;
using EcpayPaymentGenerator.Enums;

namespace EcpayPaymentGenerator.Arguments
{
    /// <summary>
    /// 訂單的參數。
    /// </summary>
    public class PaymentArgs
    {
        /// <summary>
        /// 交易編號。由「訂單編號」＋「一組 N 碼」之中英文數字混合，長度限制為 20 碼。
        /// </summary>
        public string MerchantTradeNo { get; set; }
        /// <summary>
        /// 交易金額。
        /// </summary>
        public int? TotalAmount { get; set; }
        /// <summary>
        /// 交易描述。已經過 UrlEncode 處理。
        /// </summary>
        public string TradeDesc
        {
            get => _tradeDesc;
            set => _tradeDesc = WebUtility.UrlEncode(value);
        }
        private string _tradeDesc = string.Empty;
        /// <summary>
        /// 於綠界付款完成後，回傳通知的網址。
        /// </summary>
        public string ReturnURL { get; set; }
        /// <summary>
        /// 於綠界付款完成後，將頁面導回到此設定網址的按鈕連結（無付款結果）。
        /// </summary>
        public string ClientBackURL { get; set; }
        /// <summary>
        /// 隱藏的付款方式
        /// </summary>
        public IgnorePayment? IgnorePayment { get; set; }
    }
}