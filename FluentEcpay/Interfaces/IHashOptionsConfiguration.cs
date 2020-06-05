using FluentEcpay.Enums;

namespace FluentEcpay.Interfaces
{
    public interface IHashOptionsConfiguration
    {
        IHashOptionsConfiguration Algorithm(HashAlgorithm algorithm);
    }
}