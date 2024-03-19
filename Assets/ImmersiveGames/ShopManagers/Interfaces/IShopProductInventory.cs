namespace ImmersiveGames.ShopManagers.Interfaces
{
    public interface IShopProductInventory : IShopProduct
    {
        void AddPlayerProductList( IStockShop stockShop, int quantity);
    }
}