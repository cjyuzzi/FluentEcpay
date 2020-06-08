using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using FluentEcpay.Configurations;
using FluentEcpay.Interfaces;
using FluentEcpay.Models;
using FluentEcpay.Enums;
using System.Net.Http;
using System.Net;

namespace FluentEcpay.UnitTests
{
    public class FluentEcpayTest_Payment
    {
        [Fact]
        public void CreatePayment_InputServiceAndTransaction_ReturnPayment()
        {
            // Arrange
            var now = DateTime.Now;
            var expected = new Payment
            {
                URL = "https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5",
                MerchantID = "2000132",
                ReturnURL = "https://tsem/api/payment/callback",
                ClientBackURL = "https://tsem/payment/success",
                TradeDesc = "%e6%80%a5%e8%a8%ba%e9%86%ab%e5%ad%b8%e6%9c%83%e8%b3%bc%e7%89%a9%e7%b3%bb%e7%b5%b1",
                MerchantTradeDate = now.ToString("yyyy/MM/dd HH:mm:ss"),
                ChoosePayment = "Credit",
                ItemName = "手機 20 新臺幣 x 2#隨身碟 60 新臺幣 x 1",
                TotalAmount = 100,
                PaymentType = "aio",
                EncryptType = 1,
                MerchantTradeNo = string.Empty, //dynamic
                CheckMacValue = string.Empty,// dynamic
                #region Optional
                StoreID = null,
                ItemURL = null,
                Remark = null,
                ChooseSubPayment = null,
                OrderResultURL = null,
                NeedExtraPaidInfo = null,
                DeviceSource = null,
                IgnorePayment = null,
                PlatformID = null,
                InvoiceMark = null,
                CustomField1 = null,
                CustomField2 = null,
                CustomField3 = null,
                CustomField4 = null,
                Language = null,
                #endregion
            };
            #region Input
            var service = new
            {
                Url = "https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5",
                MerchantId = "2000132",
                HashKey = "5294y06JbISpM5x9",
                HashIV = "v77hoKGq4kWxNNIS",
                ServerUrl = "https://tsem/api/payment/callback",
                ClientUrl = "https://tsem/payment/success"
            };
            var transaction = new
            {
                No = "tsem00001",
                Description = "急診醫學會購物系統",
                Date = now,
                Method = PaymentMethod.Credit,
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

            // Act
            // TODO: InvoiceMark, Language
            IPayment actual = new PaymentConfiguration()
                .Send.ToApi(
                    url: service.Url)
                .Send.ToMerchant(
                    merChantId: service.MerchantId,
                    storeId: null,
                    isPlatform: false)
                .Send.UsingHash(
                    key: service.HashKey,
                    iv: service.HashIV,
                    algorithm: HashAlgorithm.SHA256)
                .Return.ToServer(
                    url: service.ServerUrl)
                .Return.ToClient(
                    url: service.ClientUrl,
                    needExtraPaidInfo: false)
                .Transaction.New(
                    no: transaction.No,
                    description: transaction.Description,
                    date: transaction.Date,
                    remark: null)
                .Transaction.UseMethod(
                    method: transaction.Method,
                    sub: null,
                    ignore: null)
                .Transaction.WithItems(
                    items: transaction.Items,
                    url: null)
                .Transaction.WithCustomFields(
                    field1: null,
                    field2: null,
                    field3: null,
                    field4: null)
                .Generate();

            // Assert
            actual.Should().NotBeNull();
            actual.Should().BeEquivalentTo(expected, options => options
                 .Excluding(p => p.MerchantTradeNo)
                 .Excluding(p => p.CheckMacValue));
        }

        [Fact]
        public async Task PaymentCheckout_InputPayment_ReturnHttpResponse()
        {
            // Arrange
            var now = DateTime.Now;
            #region Input
            var service = new
            {
                Url = "https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5",
                MerchantId = "2000132",
                HashKey = "5294y06JbISpM5x9",
                HashIV = "v77hoKGq4kWxNNIS",
                ServerUrl = "http://example.com",
                ClientUrl = "https://tsem/payment/success"
            };
            var transaction = new
            {
                No = "tsem00003",
                Description = "急診醫學會購物系統",
                Date = now,
                Method = PaymentMethod.Credit,
                Items = new List<Item>{
                    new Item{
                        Name = "手機",
                        Price=14000,
                        Quantity = 2
                    },
                    new Item{
                        Name = "隨身碟",
                        Price = 900,
                        Quantity = 10
                    }
                }
            };
            #endregion

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

            // Act
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

            // Assert
            actual.StatusCode.Should().Be(expected);
            var html = await actual.Content.ReadAsStringAsync();
            html.Should().NotContainAny("交易失敗！ START", "常見失敗原因如下", "Transaction failed");
        }
    }
}