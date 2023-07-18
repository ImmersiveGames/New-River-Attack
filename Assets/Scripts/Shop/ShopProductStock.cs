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

        public bool AvariableToSelect(PlayerStats player)
        {
            if (PlayerAlreadyBuy(player) && shopProduct.ShouldBeConsume(player))
                return true;
            return false;
        }

        public bool AvailableInStock()
        {
            if (infinity || (!infinity && quantity > 0))
                return true;
            return false;
        }

        public bool AvariableForBuy(PlayerStats player)
        {
            if (AvailableInStock() && HaveMoneyToBuy(player) && (PlayerAlreadyBuy(player) && shopProduct.isConsumable || !PlayerAlreadyBuy(player)))
                return true;
            return false;
        }

        public bool HaveMoneyToBuy(PlayerStats player)
        {
            if (player.wealth >= shopProduct.priceItem)
                return true;
            return false;
        }
        public bool PlayerAlreadyBuy(PlayerStats player)
        {
            if (player.listProducts.Count > 0 && player.listProducts.Contains(shopProduct))
                return true;
            else
                return false;
        }
    }
}
