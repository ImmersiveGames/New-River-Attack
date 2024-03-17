using UnityEngine;
using ImmersiveGames.PlayerManagers.ScriptableObjects;

namespace ImmersiveGames.ShopManagers.Interfaces
{
    public interface IShopProduct
    {
        public string GetName();
        public string GetDescription();
        public int GetPrice();
        public Sprite GetImage();
        
    }
}