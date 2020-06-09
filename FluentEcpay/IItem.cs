namespace FluentEcpay
{
    /// <summary>
    /// 訂單的商品資料。
    /// </summary>
    public interface IItem
    {
        /// <summary>
        /// 商品名稱。
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 商品單價。
        /// </summary>
        int Price { get; set; }
        /// <summary>
        /// 購買數量。
        /// </summary>
        /// <value></value>
        int Quantity { get; set; }
    }
}