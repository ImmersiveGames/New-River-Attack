using ImmersiveGames.PlayerManagers.ScriptableObjects;
using ImmersiveGames.ShopManagers.Abstracts;
using ImmersiveGames.ShopManagers.Interfaces;
using UnityEngine;

namespace ImmersiveGames.ShopManagers
{
    [CreateAssetMenu(fileName = "ShopProductBuy", menuName = "ImmersiveGames/Shopping/ShopProductBuy", order = 201)]
    public class ShopProductBuy: ShopProduct, IShopProductBuy
    {
        public void AddPlayerProductList(PlayerSettings player, int quantity)
        {
            throw new System.NotImplementedException();
        }
    }
}