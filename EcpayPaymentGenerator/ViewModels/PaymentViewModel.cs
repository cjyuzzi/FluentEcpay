using EcpayPaymentGenerator.Arguments;

namespace EcpayPaymentGenerator.ViewModels
{
    /// <summary>
    /// 綠界金流的訂單產生器，包含介接服務所需要的所有必要及預設參數。
    /// </summary>
    public class PaymentViewModel
    {
        /// <summary>
        /// 介接服務的參數。
        /// </summary>
        /// <value></value>
        public ServiceArgs Service { get; set; } = new ServiceArgs();
    }
}