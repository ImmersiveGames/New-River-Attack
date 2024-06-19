using ImmersiveGames.DebugManagers;
using ImmersiveGames.ShopManagers.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ImmersiveGames.ShopManagers.NavigationModes
{
    public class FiniteNavigationMode : INavigationMode
    {
        public int SelectedItemIndex { get; protected set; }

        public virtual void MoveContent(RectTransform content, bool forward, MonoBehaviour monoBehaviour = null)
        {
            var rect = content.anchoredPosition;
            var childCount = content.childCount;
            if (childCount <= 0)
            {
                DebugManager.LogError<FiniteNavigationMode>($"Não há filhos criados");
                return;
            }

            var visibleChildIndex = GetVisibleChildIndex(content);
            var visibleChild = content.GetChild(visibleChildIndex);
            var moveAmount = visibleChild.GetComponent<RectTransform>().rect.width;

            const int maxPosition = 0;
            var minPosition = -((childCount - 1) * moveAmount);

            if (forward)
            {
                if (rect.x - moveAmount >= minPosition)
                {
                    rect.x -= moveAmount;
                    SelectedItemIndex = Mathf.Clamp(SelectedItemIndex + 1, 0, childCount - 1);
                }
            }
            else
            {
                if (rect.x + moveAmount <= maxPosition)
                {
                    rect.x += moveAmount;
                    SelectedItemIndex = Mathf.Clamp(SelectedItemIndex - 1, 0, childCount - 1);
                }
            }

            content.anchoredPosition = rect;

            UpdateSelectedItem(content, SelectedItemIndex);
        }

        private static int GetVisibleChildIndex(RectTransform content)
        {
            var childCount = content.childCount;
            for (var i = 0; i < childCount; i++)
            {
                var child = content.GetChild(i);
                if (child.gameObject is { activeSelf: true, activeInHierarchy: true })
                {
                    return i;
                }
            }
            return 0; // Retorna o primeiro índice se nenhum filho visível for encontrado
        }

        public virtual void UpdateSelectedItem(RectTransform content, int selectedIndex)
        {
            if (content.childCount <= 0)
            {
                DebugManager.LogError<FiniteNavigationMode>($"Não há Itens no content");
                return;
            }

            var childSelect = content.GetChild(selectedIndex);
            var activeButton = childSelect.GetComponentInChildren<Button>();
            if (activeButton != null)
            {
                var eventSystem = EventSystem.current;
                eventSystem.SetSelectedGameObject(activeButton.gameObject);
            }

            DebugManager.Log<FiniteNavigationMode>($"Item selecionado: {selectedIndex}");
        }
    }
}
