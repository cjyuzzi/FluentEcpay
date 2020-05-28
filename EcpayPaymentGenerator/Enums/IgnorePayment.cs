using System;

namespace EcpayPaymentGenerator.Enums
{
    /// <summary>
    /// 隱藏的付款方式。
    /// </summary>
    [Flags]
    public enum IgnorePayment
    {
        Credit = 0b_0000_0000,  // 0
        WebATM = 0b_0000_0001,  // 1
        ATM = 0b_0000_0010,  // 2
        CVS = 0b_0000_0100,  // 4
        BARCODE = 0b_0000_1000,  // 8
    }
}
