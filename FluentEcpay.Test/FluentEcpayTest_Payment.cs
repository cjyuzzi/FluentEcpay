using System;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace FluentEcpay.UnitTests
{
    public class FluentEcpayTest_Payment
    {
        [Fact]
        public void CreatePayment_InputServiceAndTransaction_ReturnPayment()
        {
            #region Arrange
            var now = DateTime.Now;
            #region Input
            var service = new
            {
                Url = "https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5",
                MerchantId = "2000132",
                HashKey = "5294y06JbISpM5x9",
                HashIV = "v77hoKGq4kWxNNIS",
                ServerUrl = "https://tsem.com/api/payment/callback",
                ClientUrl = "https://tsem.com/payment/success"
            };
            var transaction = new
            {
                No = "T1110623050003",
                Description = "急診醫學會購物系統",
                Date = now,
                Method = EPaymentMethod.Credit,
                Items = new List<Item>{
                    new Item{
                        Name = "手機",
                        Price=20,
                        Quantity = 2
                    },
                    new Item{
                        Name = "隨身碟",
                        Price = 60,
                        Quantity = 1
                    }
                }
            };
            #endregion
            var expected = new Payment
            {
                URL = "https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5",
                MerchantID = "2000132",
                MerchantTradeNo = string.Empty, //dynamic
                MerchantTradeDate = now.ToString("yyyy/MM/dd HH:mm:ss"),
                PaymentType = "aio",
                TotalAmount = 100,
                TradeDesc = "%e6%80%a5%e8%a8%ba%e9%86%ab%e5%ad%b8%e6%9c%83%e8%b3%bc%e7%89%a9%e7%b3%bb%e7%b5%b1",
                ItemName = "手機 20 新臺幣 x 2#隨身碟 60 新臺幣 x 1",
                ReturnURL = "https://tsem.com/api/payment/callback",
                ClientBackURL = "https://tsem.com/payment/success",
                ChoosePayment = "Credit",
                CheckMacValue = string.Empty,// dynamic
                EncryptType = 1,
                #region Optional
                StoreID = null,
                ItemURL = null,
                Remark = null,
                ChooseSubPayment = null,
                OrderResultURL = null,
                NeedExtraPaidInfo = null,
                IgnorePayment = null,
                PlatformID = null,
                InvoiceMark = null,
                CustomField1 = null,
                CustomField2 = null,
                CustomField3 = null,
                CustomField4 = null,
                Language = null,
                UnionPay = null
                #endregion
            };
            #endregion

            #region Act
            IPayment actual = new PaymentConfiguration()
                .Send.ToApi(
                    url: service.Url)
                .Send.ToMerchant(
                    merchantId: service.MerchantId,
                    storeId: null,
                    isPlatform: false)
                .Send.UsingHash(
                    key: service.HashKey,
                    iv: service.HashIV,
                    algorithm: EHashAlgorithm.SHA256)
                .Return.ToServer(
                    url: service.ServerUrl)
                .Return.ToClient(
                    url: service.ClientUrl,
                    needExtraPaidInfo: false)
                .Transaction.New(
                    no: transaction.No,
                    description: transaction.Description,
                    date: transaction.Date,
                    remark: null,
                    isNewNo: true)
                .Transaction.UseMethod(
                    method: transaction.Method,
                    sub: null,
                    ignore: null)
                .Transaction.WithItems(
                    items: transaction.Items,
                    url: null,
                    amount: null)
                .Transaction.WithCustomFields(
                    field1: null,
                    field2: null,
                    field3: null,
                    field4: null)
                .Generate();
            #endregion

            #region Assert
            actual.Should().NotBeNull();
            actual.CheckMacValue.Should().NotBeNullOrEmpty()
                .And.NotBeNullOrWhiteSpace("Because CheckMacValue will be automatically generated.");
            actual.MerchantTradeNo.Should().NotBeNullOrEmpty()
                .And.NotBeNullOrWhiteSpace("Because MerchantTradeNo will be automatically generated.");
            actual.Should().BeEquivalentTo(expected, options => options
                .Excluding(p => p.MerchantTradeNo)
                .Excluding(p => p.CheckMacValue));
            #endregion
        }

        [Fact]
        public async Task PaymentCheckout_InputPayment_ReturnHttpResponse()
        {
            #region Arrange
            var service = new
            {
                Url = "https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5",
                MerchantId = "2000132",
                HashKey = "5294y06JbISpM5x9",
                HashIV = "v77hoKGq4kWxNNIS",
                ServerUrl = "https://tsem.com/api/payment/callback",
                ClientUrl = "https://tsem.com/payment/success"
            };
            var transaction = new
            {
                No = "tsem00002",
                Description = "急診醫學會購物系統",
                Date = DateTime.Now,
                Method = EPaymentMethod.Credit,
                Items = new List<Item>{
                    new Item{
                        Name = "手機",
                        Price = 14000,
                        Quantity = 2
                    },
                    new Item{
                        Name = "隨身碟",
                        Price = 900,
                        Quantity = 10
                    }
                }
            };
            IPayment payment = new PaymentConfiguration()
                .Send.ToApi(
                    url: service.Url)
                .Send.ToMerchant(
                    service.MerchantId)
                .Send.UsingHash(
                    key: service.HashKey,
                    iv: service.HashIV)
                .Return.ToServer(
                    url: service.ServerUrl)
                .Return.ToClient(
                    url: service.ClientUrl)
                .Transaction.New(
                    no: transaction.No,
                    description: transaction.Description,
                    date: transaction.Date)
                .Transaction.UseMethod(
                    method: transaction.Method)
                .Transaction.WithItems(
                    items: transaction.Items)
                .Generate();

            var expected = HttpStatusCode.OK;
            #endregion

            #region Act
            HttpResponseMessage actual;
            var properties = typeof(IPayment).GetProperties();
            var parameters = properties
                .Where(property => property.Name != "URL")
                .Where(property => property.GetValue(payment) != null)
                .ToDictionary(property => property.Name, property => property.GetValue(payment).ToString());
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(parameters);
                var request = new HttpRequestMessage(HttpMethod.Post, payment.URL) { Content = content };
                actual = await client.SendAsync(request);
            }
            #endregion

            #region Assert
            actual.StatusCode.Should().Be(expected);
            var html = await actual.Content.ReadAsStringAsync();
            html.Should().NotContainAny("交易失敗！ START", "常見失敗原因如下", "Transaction failed");
            #endregion
        }
    }
}