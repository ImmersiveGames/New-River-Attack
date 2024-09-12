using ImmersiveGames.ShopManagers.Abstracts;

namespace ImmersiveGames.ShopManagers.Interfaces
{
    public interface IStock
    {
        ShopProduct ShopProduct { get; set;}
        int QuantityInStock { get; set; }

        void UpdateStock(int quantity);
    }
    
}