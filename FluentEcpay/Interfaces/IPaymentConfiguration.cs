using FluentEcpay.Interfaces;

namespace FluentEcpay.Interfaces
{
    public interface IPaymentConfiguration
    {
        IPaymentSendConfiguration Send { get; }
        IPaymentReturnConfiguration Return { get; }
        IPaymentTransactionConfiguration Transaction { get; }
        IPayment Generate();
    }
}