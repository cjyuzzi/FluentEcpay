using FluentEcpay.Interfaces;

namespace FluentEcpay.Configurations
{
    public class PaymentConfiguration : IPaymentConfiguration
    {
        public IPaymentSendConfiguration Send => throw new System.NotImplementedException();

        public IPaymentReturnConfiguration Return => throw new System.NotImplementedException();

        public IPaymentTransactionConfiguration Transaction => throw new System.NotImplementedException();

        public IPayment Generate()
        {
            throw new System.NotImplementedException();
        }
    }
}