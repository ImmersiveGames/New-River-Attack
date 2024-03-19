using ImmersiveGames.SaveManagers;
using ImmersiveGames.ShopManagers.Abstracts;

namespace ImmersiveGames.ShopManagers.Interfaces
{
    public interface IStockShop : IStock
    {
        ShopProductType productType { get; set; }

        bool HaveInStock(int quantity);
        bool PlayerCanBuy(GameOptionsSave gameOptionsSave, int quantity);
        bool PlayerHaveMoneyToBuy(GameOptionsSave gameOptionsSave, int quantity);
        bool PlayerAlreadyHave(GameOptionsSave gameOptionsSave, ShopProduct product);
    }
    public enum ShopProductType
    {
        Unique,
        Finite,
        Infinite
    }
}