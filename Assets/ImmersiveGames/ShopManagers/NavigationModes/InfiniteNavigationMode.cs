using ImmersiveGames.DebugManagers;
using ImmersiveGames.ShopManagers.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ImmersiveGames.ShopManagers.NavigationModes
{
    public class InfiniteNavigationMode : INavigationMode
    {
        public int SelectedItemIndex { get; private set; }

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
            SelectedItemIndex = CalculateSelectedItemIndex(content, forward);

            // Atualizar o item selecionado
            UpdateSelectedItem(content, SelectedItemIndex);
        }

        public int CalculateSelectedItemIndex(RectTransform content, bool forward)
        {
            var childCount = content.childCount;
            var selectedIndex = forward ? (SelectedItemIndex + 1) % childCount : (SelectedItemIndex - 1 + childCount) % childCount;
            SelectedItemIndex = selectedIndex;  // Atualize o índice selecionado
            return selectedIndex;
        }

        public void UpdateSelectedItem(RectTransform content, int selectedIndex)
        {
            var childSelect = content.GetChild(selectedIndex);
            var activeButton = childSelect.GetComponentInChildren<Button>();
            if (activeButton != null)
            {
                var eventSystem = EventSystem.current;
                eventSystem.SetSelectedGameObject(activeButton.gameObject);
            }
            DebugManager.Log<InfiniteNavigationMode>($"Item selecionado: {selectedIndex}");
        }
        
    }
}
