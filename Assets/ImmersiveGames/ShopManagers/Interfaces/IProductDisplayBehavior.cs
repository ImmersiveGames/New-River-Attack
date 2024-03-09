using System.Collections.Generic;

namespace ImmersiveGames.ShopManagers.Interfaces
{
    public interface IProductDisplayBehavior
    { 
        void DisplayProducts(List<ShopProductStock> shopProductsStock);
    }
}