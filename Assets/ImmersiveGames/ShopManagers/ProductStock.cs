using ImmersiveGames.ShopManagers.Abstracts;
using ImmersiveGames.ShopManagers.Interfaces;
using UnityEngine;

namespace ImmersiveGames.ShopManagers
{
    [System.Serializable]
    public struct ProductStock: IStock
    {
        [SerializeField] private int inQuantityInStock;
        [SerializeField] private ShopProduct inShopProduct;
        
        public ShopProduct shopProduct
        {
            get => inShopProduct;
            set => inShopProduct = value;
        }

        public int quantityInStock
        {
            get => inQuantityInStock;
            set => inQuantityInStock = value;
        }
    }
}