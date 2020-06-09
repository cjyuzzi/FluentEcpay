namespace FluentEcpay
{
    /// <summary>
    /// 介接綠界金流產生訂單時所需傳入的參數資料。
    /// </summary>
    public interface IPayment
    {
        /// <summary>
        /// 由綠界提供的介接路徑。分為正式環境與測試環境。
        /// </summary>        
        /// <value>String</value>
        string URL { get; set; }
        /// <summary>
        /// 由綠界提供之特店編號。
        /// </summary>
        /// <value>String(10)</value>
        string MerchantID { get; set; }
        /// <summary>
        /// 由特店提供之特店交易編號。為唯一值，不可重複使用。由「訂單編號」+「一組 N 碼」之英數字大小寫混合，如有使用 PlatformID，平台商底下所有商家之訂單編號亦不可重複。
        /// </summary>
        /// <value>String(20)</value>
        string MerchantTradeNo { get; set; }
        /// <summary>
        /// 特店旗下店舖代號。提供特店填入分店代號使用，僅可用英數字大小寫混合。
        /// </summary>
        /// <value>String(20)</value>
        string StoreID { get; set; }
        /// <summary>
        /// 特店交易時間。格式為：yyyy/MM/dd HH:mm:ss。
        /// </summary>
        /// <value>String(20)</value>
        string MerchantTradeDate { get; set; }
        /// <summary>
        /// 交易類型。請固定填入 aio。
        /// </summary>
        /// <value>String(20)</value>
        string PaymentType { get; set; }
        /// <summary>
        /// 交易金額。請帶整數，不可有小數點，僅限新台幣。測試機有付款金額的限制。
        /// </summary>
        /// <value>int</value>
        int? TotalAmount { get; set; }
        /// <summary>
        /// 交易描述。傳送到綠界前，請將參數值先做 UrlEncode。
        /// </summary>
        /// <value>String(200)</value>
        string TradeDesc { get; set; }
        /// <summary>
        /// 商品名稱。如果商品名稱有多筆，需在金流選擇頁一行一行顯示商品名稱的話，商品名稱請以符號 # 分隔。商品名稱字數限制為中英數400字內，超過此限制系統將自動截斷。
        /// </summary>
        /// <value>String(400)</value>
        string ItemName { get; set; }
        /// <summary>
        /// 付款完成通知回傳網址。當消費者付款完成後，綠界會將付款結果參數以幕後（Server POST）回傳到該網址。
        /// </summary>
        /// <value>String(200)</value>
        string ReturnURL { get; set; }
        /// <summary>
        /// 選擇預設付款方式。綠界提供下列付款方式，請於建立訂單時傳送過來：Credit：信用卡及銀聯卡（需申請開通）、WebATM：網路 ATM、ATM：自動櫃員機、CVS：超商代碼、BARCODE：超商條碼、ALL：不指定付款方式，由綠界顯示付款方式選擇頁面。
        /// </summary>
        /// <value>String(20)</value>
        string ChoosePayment { get; set; }
        /// <summary>
        /// 檢查碼。請參考文件附錄檢查碼機制與產生檢查碼範例程式。
        /// </summary>
        /// <value>String</value>
        string CheckMacValue { get; set; }
        /// <summary>
        /// Client 端返回特店的按鈕連結。消費者點選此按鈕後，會將頁面導回到此設定的網址。
        /// </summary>
        /// <value>String(200)</value>
        string ClientBackURL { get; set; }
        /// <summary>
        /// 商品銷售網址。
        /// </summary>
        /// <value>String(200)</value>
        string ItemURL { get; set; }
        /// <summary>
        /// 備註欄位。
        /// </summary>
        /// <value>String(100)</value>
        string Remark { get; set; }
        /// <summary>
        /// 付款子項目。若設定此參數，建立訂單將轉導至綠界訂單成立頁，依設定的付款方式及付款子項目帶入訂單，無法選擇其他付款子項目。
        /// </summary>
        /// <value>String (20)</value>
        string ChooseSubPayment { get; set; }
        /// <summary>
        /// Client 端回傳付款結果網址。當消費者付款完成後，綠界會將付款結果參數以幕前(Client POST)回傳到該網址。
        /// </summary>
        /// <value>String(200)</value>
        string OrderResultURL { get; set; }
        /// <summary>
        /// 是否需要額外的付款資訊。額外的付款資訊：若不回傳額外的付款資訊時，參數值請傳：Ｎ；若要回傳額外的付款資訊時，參數值請傳：Ｙ，付款完成後綠界會以 Server POST 方式回傳額外付款資訊。
        /// </summary>
        /// <value>String(1) 預設值：N</value>
        string NeedExtraPaidInfo { get; set; }
        /// <summary>
        /// 隱藏付款方式。當付款方式 <c>ChoosePayment</c> 為 ALL 時，可隱藏不需要的付款方式，多筆請以井號分隔 #。
        /// </summary>
        /// <value>String(100)</value>
        string IgnorePayment { get; set; }
        /// <summary>
        /// 由綠界提供之特約合作平台商代號。為專案合作的平台商使用。一般特店或平台商本身介接，則參數請帶放空值。若為專案合作平台商的特店使用時，則參數請帶平台商所綁的特店編號 <c>MerchantID</c>。
        /// </summary>
        /// <value>String(10)</value>
        string PlatformID { get; set; }
        /// <summary>
        /// 電子發票開立註記。此參數為付款完成後同時開立電子發票。若要使用時，該參數須設定為「Y」，同時還要設定「電子發票介接相關參數」。
        /// </summary>
        /// <value>String(1)</value>
        string InvoiceMark { get; set; }
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
        /// CheckMacValue 加密類型。請固定填入1，使用SHA256加密。
        /// </summary>
        /// <value>int</value>
        int? EncryptType { get; set; }
        /// <summary>
        /// 語系設定。預設語系為中文，若要變更語系參數值請帶： 英語：ENG、韓語：KOR、日語：JPN、簡體中文：CHI。
        /// </summary>
        /// <value></value>
        string Language { get; set; }
        /// <summary>
        /// 銀聯卡交易選項。可帶入以下選項: 0: 消費者於交易頁面可選擇是否使用銀聯交易。1: 只使用銀聯卡交易，且綠界會將交易頁面直接導到銀聯網站。2: 不可使用銀聯卡，綠界會將交易頁面隱藏銀聯選項。
        /// </summary>
        /// <value>Int 預設值：0</value>
        int? UnionPay { get; set; }
    }
}