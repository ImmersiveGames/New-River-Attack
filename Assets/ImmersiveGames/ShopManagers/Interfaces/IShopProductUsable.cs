namespace ImmersiveGames.ShopManagers.Interfaces
{
    public interface IShopProductUsable: IShopProduct
    {
        void Use( IStockShop stockShop, int quantity);
    }
}