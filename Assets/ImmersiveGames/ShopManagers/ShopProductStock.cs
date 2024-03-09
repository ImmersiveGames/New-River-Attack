using ImmersiveGames.ShopManagers.Abstracts;
using ImmersiveGames.ShopManagers.Interfaces;
using UnityEngine;

namespace ImmersiveGames.ShopManagers
{
    [System.Serializable]
    public struct ShopProductStock: IStockShop
    {
        [SerializeField] private ShopProductType inShopProductType;
        [SerializeField] private ShopProduct inShopProduct;
        [SerializeField] private int inQuantityInStock;
        
        public ShopProductType productType
        {
            get => inShopProductType;
            set => inShopProductType = value;
        }

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