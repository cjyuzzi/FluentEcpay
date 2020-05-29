using System;

namespace EcpayPaymentGenerator.Models
{
    /// <summary>
    /// 綠界金流的訂單產生器，包含介接服務所需要的所有必要及預設參數。
    /// </summary>
    public class Payment : IEquatable<Payment>
    {
        public string URL { get; set; }
        public string MerchantID { get; set; }
        public string MerchantTradeNo { get; set; }
        public string MerchantTradeDate { get; set; }
        public string CheckMacValue { get; set; }
        public string ReturnURL { get; set; }
        public string ClientBackURL { get; set; }
        public string TradeDesc { get; set; }
        public string ItemName { get; set; }
        public int TotalAmount { get; set; }
        public string ItemURL { get; set; }
        public string Remark { get; set; }
        public string ChoosePayment { get; set; }
        public string IgnorePayment { get; set; }
        public string PaymentType { get; set; }
        public int EncryptType { get; set; }

        public bool Equals(Payment other)
        {
            if (other is null)throw new ArgumentNullException(nameof(other));

            throw new NotImplementedException();
        }
    }
}