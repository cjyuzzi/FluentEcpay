using System;
using FluentEcpay.Interfaces;
using FluentEcpay.Enums;

namespace FluentEcpay.Configurations
{
    public class PaymentSendConfiguration : IPaymentSendConfiguration
    {
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