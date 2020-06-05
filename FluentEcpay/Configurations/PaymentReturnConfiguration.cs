using FluentEcpay.Interfaces;

namespace FluentEcpay.Configurations
{
    public class PaymentReturnConfiguration : IPaymentReturnConfiguration
    {
        public IPaymentConfiguration ToClient(string url, bool needExtraPaidInfo = false)
        {
            throw new System.NotImplementedException();
        }

        public IPaymentConfiguration ToServer(string url)
        {
            throw new System.NotImplementedException();
        }
    }
}