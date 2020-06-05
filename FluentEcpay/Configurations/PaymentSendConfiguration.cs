using System;
using FluentEcpay.Interfaces;
using FluentEcpay.Enums;

namespace FluentEcpay.Configurations
{
    public class PaymentSendConfiguration : IPaymentSendConfiguration
    {
        private readonly PaymentConfiguration _configuration;
        private readonly Action<string> _setUrl;
        private readonly Action<string> _setMerchantId;
        private readonly Action<string> _setHashKey;
        private readonly Action<string> _setHashIV;

        public PaymentSendConfiguration(
            PaymentConfiguration paymentConfiguration,
            Action<string> setUrl,
            Action<string> setMerchantId,
            Action<string> setHashKey,
            Action<string> setHashIV)
        {
            _configuration = paymentConfiguration;
            _setUrl = setUrl;
            _setMerchantId = setMerchantId;
            _setHashKey = setHashKey;
            _setHashIV = setHashIV;
        }

        public IPaymentConfiguration ToApi(string url)
        {
            throw new NotImplementedException();
        }

        public IPaymentConfiguration ToMerchant(string merChantId, string storeId = null, bool isPlatform = false)
        {
            throw new NotImplementedException();
        }

        public IPaymentConfiguration UsingHash(string key, string iv, HashAlgorithm algorithm = HashAlgorithm.SHA256)
        {
            throw new NotImplementedException();
        }
    }
}