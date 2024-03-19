using System.Linq;
using ImmersiveGames.SaveManagers;
using ImmersiveGames.ShopManagers.Abstracts;
using ImmersiveGames.ShopManagers.Interfaces;
using UnityEngine;

namespace ImmersiveGames.ShopManagers.ShopProducts
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
            set => inQuantityInStock = value;// = inShopProductType == ShopProductType.Unique ? 1: value;
        }
        
        public void UpdateStock(int quantity)
        {
            if (inQuantityInStock >= 0 && inShopProductType != ShopProductType.Infinite)
            {
                inQuantityInStock += quantity;
            }

            if (inShopProductType == ShopProductType.Unique && quantity > 0)
            {
                inQuantityInStock = 1;
            }
        }
        
        public bool HaveInStock(int quantity)
        {
            if (inShopProductType == ShopProductType.Infinite) return true;
            if (inQuantityInStock <= 0) return false;
            return inQuantityInStock >= quantity;
        }

        public bool PlayerAlreadyHave(GameOptionsSave gameOptionsSave, ShopProduct product)
        {
            return gameOptionsSave.listPlayerProductStocks.
                Any(objeto => objeto.inShopProduct == product);
        }

        public bool PlayerHaveMoneyToBuy(GameOptionsSave gameOptionsSave, int quantity)
        {
            return gameOptionsSave.wallet >= inShopProduct.priceItem * quantity;
        }

        public bool PlayerCanBuy(GameOptionsSave gameOptionsSave, int quantity)
        {
            if (!HaveInStock(quantity) || gameOptionsSave.wallet < inShopProduct.priceItem * quantity) return false;
            var product = inShopProduct;
            var haveProduct = PlayerAlreadyHave(gameOptionsSave, product);
            return inShopProductType != ShopProductType.Unique || !haveProduct;
        }
    }
}