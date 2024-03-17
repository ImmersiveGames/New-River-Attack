using ImmersiveGames.ShopManagers.Interfaces;
using UnityEngine;

namespace ImmersiveGames.ShopManagers.NavigationModes
{
    public class InfiniteNavigationMode : INavigationMode
    {
        private int _selectedItemIndex;

        public virtual void MoveContent(RectTransform content, bool forward, MonoBehaviour monoBehaviour = null)
        {
            var rect = content.anchoredPosition;
            var childCount = content.childCount;
            var moveAmount = content.rect.width / childCount;

            if (forward)
            {
                rect.x -= moveAmount;  // Mova continuamente para a esquerda
            }
            else
            {
                rect.x += moveAmount;  // Mova continuamente para a direita
            }

            // Ajuste a posição para manter o loop infinito
            if (rect.x > 0)
            {
                rect.x -= childCount * moveAmount;
            }
            else if (rect.x < -((childCount - 1) * moveAmount))
            {
                rect.x += childCount * moveAmount;
            }

            content.anchoredPosition = rect;

            // Calcula o índice do item centralizado após o movimento
            _selectedItemIndex = CalculateSelectedItemIndex(content, forward);

            // Atualizar o item selecionado
            UpdateSelectedItem(_selectedItemIndex);
        }

        public int CalculateSelectedItemIndex(RectTransform content, bool forward)
        {
            var childCount = content.childCount;
            var selectedIndex = forward ? (_selectedItemIndex + 1) % childCount : (_selectedItemIndex - 1 + childCount) % childCount;
            _selectedItemIndex = selectedIndex;  // Atualize o índice selecionado
            return selectedIndex;
        }

        public void UpdateSelectedItem(int selectedIndex)
        {
            // Implemente o código necessário para lidar com a seleção do item
            // por exemplo, você pode notificar o gerenciador da loja sobre a seleção do item
            // ou atualizar a visualização na interface do usuário, etc.
            Debug.Log($"Item selecionado: {selectedIndex}");
        }
    }
}
