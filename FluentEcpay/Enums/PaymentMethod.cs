using System;

namespace FluentEcpay.Enums
{
    /// <summary>
    /// 付款方式。
    /// </summary>
    [Flags]
    public enum PaymentMethod
    {
        Credit = 0,
        Union = 1,
        WebATM = 2,
        ATM = 4,
        CVS = 8,
        BARCODE = 16,
        ALL = 32
    }
}
