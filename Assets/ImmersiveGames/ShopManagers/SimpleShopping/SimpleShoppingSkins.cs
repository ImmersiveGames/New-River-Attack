using ImmersiveGames.SaveManagers;
using ImmersiveGames.ShopManagers.Interfaces;
using ImmersiveGames.ShopManagers.SimpleShopping.Abstracts;

namespace ImmersiveGames.ShopManagers.SimpleShopping
{
    public class SimpleShoppingSkins : ShopProductSettings
    {
        protected internal override void DisplayStock(IStockShop stockShop)
        {
            base.DisplayStock(stockShop);
            SelectSkinButton(stockShop, GameOptionsSave.instance);
        }

        private void SelectSkinButton(IStockShop stockShop, GameOptionsSave gameOptionsSave)
        {
            var hasSkin = stockShop.PlayerAlreadyHave(gameOptionsSave, stockShop.shopProduct);
            buttonUse.gameObject.SetActive(hasSkin);
            buttonBuy.gameObject.SetActive(!hasSkin);
        }
    }
}