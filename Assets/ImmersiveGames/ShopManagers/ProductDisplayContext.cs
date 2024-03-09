using System.Collections.Generic;
using ImmersiveGames.ShopManagers.Interfaces;

namespace ImmersiveGames.ShopManagers
{
    public class ProductDisplayContext
    {
        private IProductDisplayBehavior _displayBehavior;

        public ProductDisplayContext(IProductDisplayBehavior behavior)
        {
            SetDisplayBehavior(behavior);
        }

        private void SetDisplayBehavior(IProductDisplayBehavior behavior)
        {
            _displayBehavior = behavior;
        }

        public void DisplayProducts(List<ShopProductStock> products)
        {
            _displayBehavior?.DisplayProducts(products);
        }
    }
}