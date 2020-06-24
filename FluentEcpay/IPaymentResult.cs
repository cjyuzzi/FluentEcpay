namespace FluentEcpay
{
    /// <summary>
    /// 完成綠界金流付款回傳的結果通知之參數資料。
    /// </summary>
    public interface IPaymentResult
    {
        /// <summary>
        /// 特店編號。
        /// </summary>
        /// <value>String(10)</value>
        string MerchantID { get; set; }
        /// <summary>
        /// 特店交易編號。訂單產生時傳送給綠界的特店交易編號。英數字大小寫混合。
        /// </summary>
        /// <value>String(20)</value>
        string MerchantTradeNo { get; set; }
        /// <summary>
        /// 特店旗下店舖代號。提供特店填入分店代號使用，僅可用英數字大小寫混合。
        /// </summary>
        /// <value>String(20)</value>
        string StoreID { get; set; }
        /// <summary>
        /// 交易狀態。若回傳值為 1 時，為付款成功其餘代碼皆為交易異常，請至廠商管理後台確認後再出貨。
        /// </summary>
        /// <value>Int</value>
        int RtnCode { get; set; }
        /// <summary>
        /// 交易訊息。Server POST 成功回傳：交易成功；Server POST 補送通知回傳：paid；Client POST 成功回傳：Succeeded。
        /// </summary>
        /// <value>String(200)</value>
        string RtnMsg { get; set; }
        /// <summary>
        /// 綠界的交易編號。請保存綠界的交易編號與特店交易編號【MerchantTradeNo】的關連。
        /// </summary>
        /// <value>String(20)</value>
        string TradeNo { get; set; }
        /// <summary>
        /// 交易金額。
        /// </summary>
        /// <value>Int</value>
        int TradeAmt { get; set; }
        /// <summary>
        /// 付款時間。格式為：yyyy/MM/dd HH:mm:ss。
        /// </summary>
        /// <value>String(20)</value>
        string PaymentDate { get; set; }
        /// <summary>
        /// 特店選擇的付款方式。
        /// </summary>
        /// <value>String(20)</value>
        string PaymentType { get; set; }
        /// <summary>
        /// 通路費。
        /// </summary>
        /// <value>Int</value>
        int PaymentTypeChargeFee { get; set; }
        /// <summary>
        /// 訂單成立時間。格式為：yyyy/MM/dd HH:mm:ss。
        /// </summary>
        /// <value>String(20)</value>
        string TradeDate { get; set; }
        /// <summary>
        /// 是否為模擬付款。回傳值：若為 1 時，代表此交易為模擬付款，請勿出貨。若為 0 時，代表此交易非模擬付款。
        /// </summary>
        /// <value>Int</value>
        int SimulatePaid { get; set; }
        /// <summary>
        /// 自訂名稱欄位 1。提供合作廠商使用記錄用客製化使用欄位。
        /// </summary>
        /// <value>String(50)</value>
        string CustomField1 { get; set; }
        /// <summary>
        /// 自訂名稱欄位 2。提供合作廠商使用記錄用客製化使用欄位。
        /// </summary>
        /// <value>String(50)</value>
        string CustomField2 { get; set; }
        /// <summary>
        /// 自訂名稱欄位 3。提供合作廠商使用記錄用客製化使用欄位。
        /// </summary>
        /// <value>String(50)</value>
        string CustomField3 { get; set; }
        /// <summary>
        /// 自訂名稱欄位 4。提供合作廠商使用記錄用客製化使用欄位。
        /// </summary>
        /// <value>String(50)</value>
        string CustomField4 { get; set; }
        /// <summary>
        /// 檢查碼。特店必須檢查檢查碼來驗證。
        /// </summary>
        /// <value>String</value>
        string CheckMacValue { get; set; }
    }
}