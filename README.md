# FluentEcpay

使用 [Fluent Interface](https://zh.wikipedia.org/wiki/%E6%B5%81%E5%BC%8F%E6%8E%A5%E5%8F%A3) 風格的設計，產生綠界金流的交易訂單。

* 綠界金流文件版本：[V 5.2.3](https://www.ecpay.com.tw/Service/API_Dwnld)
* 參考官方 SDK 設計：[.NET SDK](https://github.com/ECPay/ECpayAIO_Net)

## 設定

1. Namespace : 
```csharp=
using FluentEcpay;
```
2. 建立 `PaymentConfiguration` 並設定參數 : 
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
        remark: null)
    .Transaction.UseMethod(
        method: <Method>,
        sub: null,
        ignore: null)
    .Transaction.WithItems(
        items: <Items>,
        url: null)
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
4. 將訂單資料傳送到 View 之後，將會自動轉址到綠界金流支付平台。
