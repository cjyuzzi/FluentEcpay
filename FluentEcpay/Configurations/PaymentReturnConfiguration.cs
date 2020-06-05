using System;
using FluentEcpay.Interfaces;

namespace FluentEcpay.Configurations
{
    public class PaymentReturnConfiguration : IPaymentReturnConfiguration
    {
        private readonly PaymentConfiguration _configuration;
        private readonly Action<string> _setServerUrl;
        private readonly Action<string> _setClientUrl;

        public PaymentReturnConfiguration(
            PaymentConfiguration configuration,
            Action<string> setServerUrl,
            Action<string> setClientUrl)
        {
            _configuration = configuration;
            _setServerUrl = setServerUrl;
            _setClientUrl = setClientUrl;
        }

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