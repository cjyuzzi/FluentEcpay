namespace FluentEcpay.Interfaces
{
    public interface IItem
    {
        string Name { get; set; }
        int Price { get; set; }
        int Quantity { get; set; }
    }
}