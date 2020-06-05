namespace FluentEcpay.Interfaces
{
    public interface IMerchantOptionsConfiguration
    {
        IMerchantOptionsConfiguration ToStore(string id);
        IMerchantOptionsConfiguration IsPlatform();
    }
}