using UnityEngine;

namespace ImmersiveGames.ShopManagers.Interfaces
{
    public interface IShopLayout
    {
        void ConfigureLayout(RectTransform content, int itemCount, GameObject prefabItemShop);
        void ResetContentPosition(RectTransform content);
    }
}