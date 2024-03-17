using ImmersiveGames.SaveManagers;
using ImmersiveGames.ShopManagers.Abstracts;

namespace ImmersiveGames.ShopManagers.Interfaces
{
    public interface IStockShop : IStock
    {
        ShopProductType productType { get; set; }

        bool HaveInStock();
        bool PlayerCanBuy(GameOptionsSave gameOptionsSave);
        bool PlayerAlreadyHave(GameOptionsSave gameOptionsSave, ShopProduct product);
    }
    public enum ShopProductType
    {
        Unique,
        Finite,
        Infinite
    }
}