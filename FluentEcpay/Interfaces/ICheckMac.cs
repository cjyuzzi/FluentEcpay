
using FluentEcpay.Enums;

namespace FluentEcpay.Interfaces
{
    /// <summary>
    /// 綠界服務介接的檢查碼機制。
    /// </summary>
    public interface ICheckMac
    {
        /// <summary>
        /// 產生檢查碼。
        /// </summary>
        string GetValue(IPayment payment, string hashKey, string hashIV, EHashAlgorithm encryptType);
    }
}