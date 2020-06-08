
using FluentEcpay.Enums;

namespace FluentEcpay.Interfaces
{
    public interface ICheckMac
    {
        string GetValue(IPayment payment, string hashKey, string hashIV, EHashAlgorithm encryptType);
    }
}