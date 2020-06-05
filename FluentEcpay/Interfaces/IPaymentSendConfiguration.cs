using System;

namespace FluentEcpay.Interfaces
{
    public interface IPaymentSendConfiguration
    {
        IPaymentConfiguration ToApi(string url);
        IPaymentConfiguration ToMerchant(string id, Action<IMerchantOptionsConfiguration> configureMerchant);
        IPaymentConfiguration UsingHash(string key, string iv, Action<IHashOptionsConfiguration> configureHash);
    }
}