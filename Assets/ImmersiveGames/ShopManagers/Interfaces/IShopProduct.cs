using UnityEngine;
using ImmersiveGames.PlayerManagers.ScriptableObjects;

namespace ImmersiveGames.ShopManagers.Interfaces
{
    public interface IShopProduct
    {
        public string GetName(bool locale);
        public string GetDescription(bool locale);
        public int GetPrice();
        public Sprite GetImage();
        public void Buy(PlayerSettings player);
        
        
    }

    
}