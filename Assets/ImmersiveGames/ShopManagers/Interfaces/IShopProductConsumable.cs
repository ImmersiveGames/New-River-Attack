using ImmersiveGames.PlayerManagers.ScriptableObjects;

namespace ImmersiveGames.ShopManagers.Interfaces
{
    public interface IShopProductConsumable: IShopProduct
    {
        void Expend( PlayerSettings player, int quantity);
    }
}