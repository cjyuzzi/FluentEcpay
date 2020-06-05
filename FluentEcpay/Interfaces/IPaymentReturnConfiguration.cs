using System;

namespace FluentEcpay.Interfaces
{
    public interface IPaymentReturnConfiguration
    {
        IPaymentConfiguration ToServer(string url);
        IPaymentConfiguration ToClient(string url, Action<IClientOptionsConfiguration> configureClient);
    }
}