using System;

namespace FluentEcpay.Enums
{
    /// <summary>
    /// 隱藏付款方式。
    /// </summary>
    [Flags]
    public enum EPaymentIgnoreMethod
    {
        Credit = 0,
        WebATM = 1,
        ATM = 2,
        CVS = 4,
        BARCODE = 8
    }
}