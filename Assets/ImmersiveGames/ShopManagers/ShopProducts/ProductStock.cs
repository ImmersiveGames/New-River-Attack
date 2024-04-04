using ImmersiveGames.ShopManagers.Abstracts;
using ImmersiveGames.ShopManagers.Interfaces;
using UnityEngine;

namespace ImmersiveGames.ShopManagers.ShopProducts
{
    [System.Serializable]
    public struct ProductStock: IStock
    {
        [SerializeField] private int inQuantityInStock;
        [SerializeField] internal ShopProduct inShopProduct;
        
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

        public void UpdateStock(int quantity)
        {
            if (inQuantityInStock >= 0)
            {
                inQuantityInStock += quantity;
            }
        }
    }
}