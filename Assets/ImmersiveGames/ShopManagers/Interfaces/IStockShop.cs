namespace ImmersiveGames.ShopManagers.Interfaces
{
    public interface IStockShop : IStock
    {
        ShopProductType productType { get; set; }
    }
    public enum ShopProductType
    {
        Unique,
        Finite,
        Infinite
    }
}