using Xunit;
using EcpayPaymentGenerator.Configurations;
using EcpayPaymentGenerator.Models;
using System.Collections.Generic;
using FluentAssertions;

namespace EcpayPaymentGenerator.UnitTests.Configurations
{
    public class EcpayPaymentGenerator_CreatePayment
    {
        public EcpayPaymentGenerator_CreatePayment()
        {

        }

        [Fact]
        public void CreatePayment_InputIsConfiguration_ReturnPayment()
        {
            // Arrange
            var expected = new Payment
            {
                URL = "https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5",
                MerchantID = "2000132",
                MerchantTradeNo = "",
                MerchantTradeDate = "",
                CheckMacValue = "",
                ReturnURL = "/api/callback",
                ClientBackURL = "https://google.com.tw",
                TradeDesc = "急診醫學會購物系統",
                ItemName = "手機 20 新台幣 x 2#隨身碟 60 新台幣 x 1",
                TotalAmount = 100,
                ItemURL = "https://google.com.tw",
                Remark = "Remark",
                ChoosePayment = "ALL",
                IgnorePayment = "ATM#CVS",
                PaymentType = "aio",
                EncryptType = 1
            };
            var items = new List<Item>{
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
            };
            // Act
            Payment actual = new EcpayConfiguration()
                .SendTo.Api("https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5", "2000132", "5294y06JbISpM5x9", "v77hoKGq4kWxNNIS")
                .ReturnTo.Action("/api/callback")
                .ReturnTo.Client("https://google.com.tw", false)
                .CreatePayment("tsem", "急診醫學會購物系統", items, "https://google.com.tw", "Remark");

            // Assert
            actual.Should().NotBeNull();
            actual.Should().BeEquivalentTo(expected, options =>
                options.Excluding(o => o.MerchantTradeNo)
                    .Excluding(o => o.MerchantTradeDate)
            );
        }
    }
}