using FluentEcpay.Interfaces;

namespace FluentEcpay.Configurations
{
    public class PaymentConfiguration : IPaymentConfiguration
    {
        private string _url;
        private string _merchantId;
        private string _hashKey;
        private string _hashIV;
        private string _serverUrl;
        private string _clientUrl;
        private IPayment _payment;

        public IPaymentSendConfiguration Send { get; }

        public IPaymentReturnConfiguration Return { get; }

        public IPaymentTransactionConfiguration Transaction { get; }

        public PaymentConfiguration()
        {
            Send = new PaymentSendConfiguration(
                this,
                url => _url = url,
                id => _merchantId = id,
                key => _hashKey = key,
                iv => _hashIV = iv
            );
            Return = new PaymentReturnConfiguration(
                this,
                url => _serverUrl = url,
                url => _clientUrl = url
            );
            Transaction = new PaymentTransactionConfiguration(
                this,
                payment => _payment = payment
            );
        }

        public IPayment Generate()
        {
            throw new System.NotImplementedException();
        }
    }
}