using FluentEcpay.Enums;

namespace FluentEcpay.Interfaces
{
    public interface IPaymentSendConfiguration
    {
        IPaymentConfiguration ToApi(string url);
        IPaymentConfiguration ToMerchant(string merChantId, string storeId = null, bool isPlatform = false);
        IPaymentConfiguration UsingHash(string key, string iv, HashAlgorithm algorithm = HashAlgorithm.SHA256);
    }
}