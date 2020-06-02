using System;

namespace EcpayPaymentGenerator.Enums
{
    /// <summary>
    /// 隱藏的付款方式。
    /// </summary>
    [Flags]
    public enum IgnorePayment
    {
        Credit = 0,
        WebATM = 1,
        ATM = 2,
        CVS = 4,
        BARCODE = 8
    }
}
