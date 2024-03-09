using ImmersiveGames.PlayerManagers.ScriptableObjects;

namespace ImmersiveGames.ShopManagers.Interfaces
{
    public interface IShopProductBuy : IShopProduct
    {
        void AddPlayerProductList( PlayerSettings player, int quantity);
    }
}