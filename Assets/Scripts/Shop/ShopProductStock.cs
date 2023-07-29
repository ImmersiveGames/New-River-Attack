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
            return AvailableInStock() && HaveMoneyToBuy(player) && (PlayerAlreadyBuy(player) && shopProduct.isConsumable || !PlayerAlreadyBuy(player));
        }

        public bool HaveMoneyToBuy(PlayerSettings player)
        {
            return player.wealth >= shopProduct.priceItem;
        }
        public bool PlayerAlreadyBuy(PlayerSettings player)
        {
            return player.listProducts.Count > 0 && player.listProducts.Contains(shopProduct);
        }
    }
}
