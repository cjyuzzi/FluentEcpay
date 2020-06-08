using FluentEcpay.Enums;

namespace FluentEcpay.Interfaces
{
    public interface IPaymentSendConfiguration
    {
        IPaymentConfiguration ToApi(string url);
        IPaymentConfiguration ToMerchant(string merchantId, string storeId = null, bool isPlatform = false);
        IPaymentConfiguration UsingHash(string key, string iv, EHashAlgorithm? algorithm = EHashAlgorithm.SHA256);
    }
}