# FluentEcpay

###### tags: `ASP.NET Core`

使用 [Fluent Interface](https://zh.wikipedia.org/wiki/%E6%B5%81%E5%BC%8F%E6%8E%A5%E5%8F%A3) 風格的設計，產生綠界金流的交易訂單，設定起來簡單、直覺又方便。

* 函式庫版本：.Net Standard 2.0。沒有相依任何其他第三方套件。
* 測試程式與範例 Web 專案版本：.NET Core 3.1。
* 綠界金流 API 串接文件版本：[V 5.2.3][doc]。
* 參考官方 SDK 設計：[.NET SDK](https://github.com/ECPay/ECpayAIO_Net)。

## 設定、產生與送出訂單

1. Namespace : 
```csharp=
using FluentEcpay;
```
2. 建立 `PaymentConfiguration` 並設定參數：參數請參考[官方 API 串接文件](https://www.ecpay.com.tw/Content/files/ecpay_011.pdf)。
```csharp=
IPayment actual = new PaymentConfiguration()
    .Send.ToApi(
        url: <URL>)
    .Send.ToMerchant(
        merchantId: <MerchantId>,
        storeId: null,
        isPlatform: false)
    .Send.UsingHash(
        key: <HashKey>,
        iv: <HashIV>,
        algorithm: EHashAlgorithm.SHA256)
    .Return.ToServer(
        url: <ServerUrl>)
    .Return.ToClient(
        url: <ClientUrl>,
        needExtraPaidInfo: false)
    .Transaction.New(
        no: <TradeNo>,
        description: <TradeDescription>,
        date: <TradeDate>,
        remark: null,
        isNewNo: true)
    .Transaction.UseMethod(
        method: <Method>,
        sub: null,
        ignore: null)
    .Transaction.WithItems(
        items: <Items>,
        url: null,
        amount: null) // 可選擇直接指定總金額不透過商品清單計算。
    .Transaction.WithCustomFields(
        field1: null,
        field2: null,
        field3: null,
        field4: null)
    .Generate();
```
3. 導向 View 並傳入 `IPayment` 物件 : View 的設計請參考 FluentEcpay.Web 專案內之 ~/Views/Payment/Checkout.cshtml。
```csharp=
 return View(payment);
```
> 使用舊的 Razor 語法請參考~/Views/Payment/Checkout_Old.cshtml。
4. 將訂單資料傳送到 View 之後，將會自動轉址到綠界金流支付平台。

## 付款結果通知

設計 API 提供綠界通知付款結果。程式碼請參考 FluentEcpay.Web 專案內 ~/Controllers/PaymentController.cs

此套件提供了 `PaymentResult` 物件來接收付款結果，詳細參數使用方式請參考[官方文件][doc]。
```csharp=
[HttpPost("callback")]
public IActionResult Callback(PaymentResult result)
{
    // 務必判斷檢查碼是否正確。
    if (!CheckMacValueIsValid(result.CheckMacValue)) return BadRequest();

    // 處理後續訂單狀態的更動等等...。

    return Ok("1|OK");
}
```

### 獲得 PaymentResult 的 CheckMacValue

此套件提供了 CheckMac 物件可以傳入 `IPaymentResult` 物件即可計算出檢查碼（實驗中），用來判斷檢查碼是否正確。

```csharp=
var checkMacValue = CheckMac.GetValue(paymentResult, hashKey, hashIV);
```

## 協助

此套件為 OpenSource 開源套件，有在串接綠界金流並且對此套件有興趣的朋友歡迎開 Issue 或是發送 PR，希望能藉由社群的力量，一起讓這個套件更強大！目前僅擁有最最最基礎的功能：產生訂單、送出訂單。

預計開發項目：
* 發票設定
* 語系設定
* 各付款方式的進階設定
* 子支付方式的列舉選項
* 還沒想到 ...

[doc]:https://www.ecpay.com.tw/Service/API_Dwnld
