using FluentEcpay.Enums;

namespace FluentEcpay.Interfaces
{
    public interface IMethodOptionsConfiguration
    {
        IMethodOptionsConfiguration Sub(PaymentSubMethod subMethod);
        IMethodOptionsConfiguration Ignore(PaymentIgnoreMethod ignoreMethod);
    }
}