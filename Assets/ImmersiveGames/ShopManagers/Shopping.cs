using System.Collections.Generic;
using ImmersiveGames.ShopManagers.Interfaces;
using UnityEngine;

namespace ImmersiveGames.ShopManagers
{
    public class Shopping: MonoBehaviour
    {
        [SerializeField] private List<ShopProductStock> productStocks;

        private void Start()
        {
            // Escolher qual comportamento de exibição usar na construção da loja
            IProductDisplayBehavior displayBehavior = new CarouselDisplayBehavior();
            // IProductDisplayBehavior displayBehavior = new ShelfDisplayBehavior();

            // Construir loja com o comportamento escolhido
            BuildStore(displayBehavior);
        }

        private void BuildStore(IProductDisplayBehavior displayBehavior)
        {
            // Criar contexto de exibição com o comportamento escolhido
            var displayContext = new ProductDisplayContext(displayBehavior);

            // Exibir produtos no formato escolhido (carrossel ou prateleira)
            displayContext.DisplayProducts(productStocks);
        }
    }
}