namespace FluentEcpay.Arguments
{
    public class ServiceArgs
    {
        /// <summary>
        /// 介接路徑。
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        /// 特店編號。
        /// </summary>
        public string MerchantID { get; set; }
        /// <summary>
        /// 檢查碼機制在產生檢核碼計算時，所需的必要參數。
        /// </summary>
        public string HashKey { get; set; }
        /// <summary>
        /// 檢查碼機制在產生檢核碼計算時，所需的必要參數。
        /// </summary>
        public string HashIV { get; set; }
    }
}