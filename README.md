# FluentEcpay

###### tags: `ASP.NET Core`

使用 [Fluent Interface](https://zh.wikipedia.org/wiki/%E6%B5%81%E5%BC%8F%E6%8E%A5%E5%8F%A3) 的風格設計，用來產生綠界金流的交易訂單，設定起來簡單、直覺又方便。

* 當前穩定版本為：1.2.1。（請各位盡速升級）
* 函式庫架構：.Net Standard 2.0。沒有相依任何其他第三方套件。
* 測試程式與範例 Web 專案版本：.NET Core 3.1。
* 綠界金流 API 串接文件版本：[V 5.2.3][doc]。
* 參考官方 SDK 設計：[.NET SDK](https://github.com/ECPay/ECpayAIO_Net)。

## 訂單的建立與送出

1. **引用** Namespace : 
```csharp=
using FluentEcpay;
```
2. **設定**與**建立訂單**：透過設定 `PaymentConfiguration` 物件並傳入特定參數產生相應的綠界金流訂單，以下為 `<>` 的參數皆為**必填欄位**。詳細參數的說明請參考[官方的 API 串接文件][doc]。
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
        amount: null) // 可選擇直接指定總金額而不透過商品清單計算。
    .Transaction.WithCustomFields(
        field1: null,
        field2: null,
        field3: null,
        field4: null)
    .Generate(); // 產生訂單物件。
```
3. **送出訂單**：導向 View 並傳入 `IPayment` 物件，View 的設計請參考 FluentEcpay.Web 專案內的範例頁面：~/Views/Payment/Checkout.cshtml。
```csharp=
 return View(payment);
```
> 使用舊的 Razor 語法請參考~/Views/Payment/Checkout_Old.cshtml。
4. 將訂單資料傳送到該 View 之後，將會自動送出訂單至綠界金流的支付平台。

## 付款結果通知

專案有提供綠界付款結果通知的範例程式碼。可參考 FluentEcpay.Web 專案中 ~/Controllers/PaymentController.cs 內的 `Callback()` Action。

FluentEcpay 提供了 `PaymentResult` 物件來負責接收付款結果，詳細的參數說明請參考[官方文件][doc]。
```csharp=
[HttpPost("callback")]
public IActionResult Callback(PaymentResult result)
{
    var hashKey = "5294y06JbISpM5x9";
    var hashIV = "v77hoKGq4kWxNNIS";
    
    // 務必判斷檢查碼是否正確。
    if (!CheckMac.PaymentResultIsValid(result, hashKey, hashIV)) return BadRequest();

    // 處理後續訂單狀態的更動等等...。

    return Ok("1|OK");
}
```

### 驗證 PaymentResult

FluentEcpay 提供了**靜態方法** `CheckMac.PaymentResultIsValid()` 來驗證付款結果是否為有效，傳入 `IPaymentResult` 物件參數即可完成檢查碼的驗證，用來判斷檢查碼是否正確。

```csharp=
// 將得到的 PaymentResult 物件
var paymentResult = GetPaymentResult(); 
// 進行驗證
bool isValid = CheckMac.PaymentResultIsValid(paymentResult, hashKey, hashIV);
```

## 共同開發

FluentEcpay 為**開源的函式庫**，有在串接綠界金流並且對此套件有興趣的朋友歡迎開 Issue 或是發 PR 一起經營，希望能藉由社群的力量，一起讓這套函式庫更加強大！

未來預計開發項目：
* 發票設定
* 語系設定
* 各付款方式的進階設定
* 子支付方式的列舉選項
* 還沒想到 ...

[doc]:https://www.ecpay.com.tw/Content/files/ecpay_011.pdf
