﻿using ImmersiveGames.SaveManagers;
using ImmersiveGames.ShopManagers.Abstracts;
using ImmersiveGames.ShopManagers.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace ImmersiveGames.ShopManagers.ShopProducts
{
    [CreateAssetMenu(fileName = "itemSkin", menuName = "ImmersiveGames/Shopping/Skins", order = 203)]
    public class ShopProductSkin : ShopProduct, IShopProductInventory, IShopProductUsable
    {
        [Header("Skin fields")]
        [SerializeField]
        public GameObject prefabSkin;
        public void AddPlayerProductList(IStockShop stockShop, int quantity)
        {
            GameOptionsSave.instance.AddInventory(stockShop.shopProduct, 1);
        }

        public void Use(IStockShop stockShop, int quantity)
        {
            //TODO: Aqui precisa avisar o objeto do player que trocou de skin??
            GameOptionsSave.instance.actualSkin = this;
        }
    }
}