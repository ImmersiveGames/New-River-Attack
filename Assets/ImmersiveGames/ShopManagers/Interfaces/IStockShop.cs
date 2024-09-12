using ImmersiveGames.ShopManagers.Abstracts;
using NewRiverAttack.SaveManagers;

namespace ImmersiveGames.ShopManagers.Interfaces
{
    public interface IStockShop : IStock
    {
        ShopProductType ProductType { get; set; }

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