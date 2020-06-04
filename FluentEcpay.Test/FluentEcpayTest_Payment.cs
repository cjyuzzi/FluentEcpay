using System;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using FluentEcpay.Configurations;
using FluentEcpay.interfaces;
using FluentEcpay.Models;
using FluentEcpay.Enums;

namespace FluentEcpay.UnitTests
{
    public class FluentEcpayTest_Payment
    {
        [Fact]
        public void CreatePayment_InputIsConfiguration_ReturnPayment()
        {
            // Arrange
            var now = DateTime.Now;
            var expected = new Payment
            {
                URL = "https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5",
                MerchantID = "2000132",
                MerchantTradeDate = now.ToString("yyyy/MM/dd HH:mm:ss"),
                TotalAmount = 100,
                TradeDesc = "急診醫學會購物系統",
                ItemName = "手機 20 新台幣 x 2#隨身碟 60 新台幣 x 1",
                ReturnURL = "https://tsem/api/payment/callback",
                ChoosePayment = "Credit",
                ClientBackURL = "https://tsem/payment/success",
                ItemURL = "https://tsem/item/1",
                Remark = "去糖去冰",
                IgnorePayment = "ATM#CVS",
                CustomField1 = "TestCustomField1",
                CustomField2 = "TestCustomField2",
                CustomField3 = "TestCustomField3",
                CustomField4 = "TestCustomField4",
                EncryptType = 1,
                Language = "ENG",
                PaymentType = "aio",
                MerchantTradeNo = null,
                CheckMacValue = null,
                StoreID = null,
                ChooseSubPayment = null,
                OrderResultURL = null,
                NeedExtraPaidInfo = null,
                DeviceSource = null,
                PlatformID = null,
                InvoiceMark = null,
            };

            var transaction = new
            {
                Method = PaymentMethod.Credit,
                IgnoreMethod = PaymentMethod.ATM | PaymentMethod.CVS,
                // TODO: No = "",
                Description = "急診醫學會購物系統",
                Date = now,
                Remark = "去糖去冰",
                ItemUrl = "https://tsem/item/1",
                Items = new List<Item>{
                    new Item{
                        Name = "手機",
                        Price=20,
                        Quantity = 2
                    },
                    new Item{
                        Name = "隨身碟",
                        Price = 60,
                        Quantity=1
                    }
                }
            };
            var service = new
            {
                Url = "https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5",
                MerchantId = "2000132",
                ServerUrl = "https://tsem/api/payment/callback",
                ClientUrl = "https://tsem/payment/success",
                HashKey = "5294y06JbISpM5x9",
                HashIV = "v77hoKGq4kWxNNIS"
            };

            // Act
            IPayment actual = new PaymentConfiguration()
                .Send.ToApi(url: service.Url)
                .Send.ToMerchant(id: service.MerchantId, isPlatform: false)
                .Send.ToStore(id: null)
                .Send.UsingHash(key: service.HashKey, iv: service.HashIV, algorithm: HashAlgorithm.SHA256)
                .Return.ToServer(url: service.ServerUrl)
                .Return.ToClient(url: service.ClientUrl, needExtraPaidInfo: false)
                .Transaction.New(
                    no: transaction.No,
                    description: transaction.Description,
                    date: transaction.Date,
                    remark: transaction.Remark)
                .Transaction.WithItems(
                    items: transaction.Items,
                    url: transaction.ItemUrl)
                .Transaction.UseMethod(
                    method: transaction.Method,
                    configure: options => options
                        .Sub(transaction.SubMethod)
                        .Ignore(transaction.IgnoreMethod))
                .Generate();
                // TODO: CustomField, Invoice, Language

            // Assert
            actual.Should().NotBeNull();
            actual.Should().BeEquivalentTo(expected);
        }
    }
}