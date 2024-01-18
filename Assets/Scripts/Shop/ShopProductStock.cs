using UnityEngine;
using RiverAttack;
namespace Shopping
{
    [System.Serializable]
    public struct ShopProductStock
    {
        [SerializeField]
        public bool infinity;
        [SerializeField]
        public int quantity;
        [SerializeField]
        public ShopProduct shopProduct;

        public void RemoveStock(int qnt)
        {
            if (!infinity)
                quantity -= qnt;
            if (quantity < 0)
                quantity = 0;
        }
        public bool AvailableToSelect(PlayerSettings player)
        {
            return PlayerAlreadyBuy(player) && shopProduct.ShouldBeConsume(player);
        }

        public bool AvailableInStock()
        {
            return infinity || (!infinity && quantity > 0);
        }

        public bool AvailableForBuy(PlayerSettings player)
        {
            // Debug.Log($"In stock {AvailableInStock()} - {shopProduct.name}");
            // Debug.Log($"In Have Money to buy {HaveMoneyToBuy(player)} - {shopProduct.name}");
            // Debug.Log($"Already BUY {PlayerAlreadyBuy(player)} - {shopProduct.name}");
            // Debug.Log($"Consumível {shopProduct.isConsumable} - {shopProduct.name}");
            if (AvailableInStock() && HaveMoneyToBuy(player))
            {
                return !PlayerAlreadyBuy(player) || shopProduct.isConsumable;
            }
            return false;
            
            // return (AvailableInStock() && HaveMoneyToBuy(player) && (PlayerAlreadyBuy(player) && shopProduct.isConsumable) || !PlayerAlreadyBuy(player));
        }

        public bool HaveMoneyToBuy(PlayerSettings player)
        {
            //Debug.Log($"Dinheiro {player.wealth}, {shopProduct.priceItem}");
            return player.wealth >= shopProduct.priceItem;
        }
        public bool PlayerAlreadyBuy(PlayerSettings player)
        {
            return player.listProducts.Count > 0 && player.listProducts.Contains(shopProduct);
        }
    }
}
