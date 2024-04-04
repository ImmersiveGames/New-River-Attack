namespace ImmersiveGames.ShopManagers.Interfaces
{
    public interface IShopProductInventory : IShopProduct
    {
        void AddPlayerProductList( int indexPlayer, IStockShop stockShop, int quantity);
    }
}