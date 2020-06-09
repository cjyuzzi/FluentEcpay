using FluentEcpay.Interfaces;

namespace FluentEcpay
{
    public class Payment : IPayment
    {
        public string URL { get; set; }
        public string MerchantID { get; set; }
        public string MerchantTradeNo { get; set; }
        public string StoreID { get; set; }
        public string MerchantTradeDate { get; set; }
        public string PaymentType { get; set; }
        public int? TotalAmount { get; set; }
        public string TradeDesc { get; set; }
        public string ItemName { get; set; }
        public string ReturnURL { get; set; }
        public string ChoosePayment { get; set; }
        public string CheckMacValue { get; set; }
        public string ClientBackURL { get; set; }
        public string ItemURL { get; set; }
        public string Remark { get; set; }
        public string ChooseSubPayment { get; set; }
        public string OrderResultURL { get; set; }
        public string NeedExtraPaidInfo { get; set; }
        public string IgnorePayment { get; set; }
        public string PlatformID { get; set; }
        public string InvoiceMark { get; set; }
        public string CustomField1 { get; set; }
        public string CustomField2 { get; set; }
        public string CustomField3 { get; set; }
        public string CustomField4 { get; set; }
        public int? EncryptType { get; set; }
        public string Language { get; set; }
        public int? UnionPay { get; set; }
    }
}