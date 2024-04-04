namespace ImmersiveGames.ShopManagers.Interfaces
{
    public interface IShopProductUsable: IShopProduct
    {
        void Use( int indexPlayer, IStockShop stockShop, int quantity);
    }
}