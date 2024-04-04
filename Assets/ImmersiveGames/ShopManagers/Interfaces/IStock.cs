using ImmersiveGames.ShopManagers.Abstracts;

namespace ImmersiveGames.ShopManagers.Interfaces
{
    public interface IStock
    {
        ShopProduct shopProduct { get; set;}
        int quantityInStock { get; set; }

        void UpdateStock(int quantity);
    }
    
}