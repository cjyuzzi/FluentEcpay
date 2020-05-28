using Xunit;
using EcpayPaymentGenerator.Configurations;
using EcpayPaymentGenerator.ViewModels;

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
            var config = new EcpayConfiguration()
                .SendTo.Api("https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5", "2000132", "5294y06JbISpM5x9", "v77hoKGq4kWxNNIS")
                .ReturnTo.Action("/api/callback")
                .ReturnTo.Client("http://google.com", false);

            var expected = new PaymentViewModel
            {

            };
            // Act
            // MerchantTradeNo, MerchantTradeDate, PaymentType, TotalAmount, ChoosePayment, CheckMacValue, DeviceSource,EncryptType
            PaymentViewModel actual = config.CreatePayment("Description", "Items", "ClientBackURL", "Remark", "IgnorePayment");

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}