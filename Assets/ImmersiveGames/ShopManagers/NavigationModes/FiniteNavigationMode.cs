using ImmersiveGames.ShopManagers.Interfaces;
using UnityEngine;

namespace ImmersiveGames.ShopManagers.NavigationModes
{
    public class FiniteNavigationMode : INavigationMode
    {
        public virtual void MoveContent(RectTransform content, bool forward, MonoBehaviour monoBehaviour = null)
        {
            var rect = content.anchoredPosition;
            var childCount = content.childCount;
            var moveAmount = content.rect.width / childCount;
            const int maxPosition = 0;
            var minPosition = -((childCount - 1) * moveAmount);

            if (forward)
            {
                if (rect.x - moveAmount >= minPosition)
                {
                    rect.x -= moveAmount;
                }
            }
            else
            {
                if (rect.x + moveAmount <= maxPosition)
                {
                    rect.x += moveAmount;
                }
            }

            content.anchoredPosition = rect;

            var selectedIndex = CalculateSelectedItemIndex(content, forward);
            UpdateSelectedItem(selectedIndex);
        }

        public int CalculateSelectedItemIndex(RectTransform content, bool forward)
        {
            var childCount = content.childCount;
            var selectedIndex = forward
                ? Mathf.Clamp(childCount - 1, 0, childCount - 1)
                : Mathf.Clamp(0, 0, content.childCount - 1);
            return selectedIndex;
        }

        public virtual void UpdateSelectedItem(int selectedIndex)
        {
            Debug.Log($"Item selecionado: {selectedIndex}");
        }
    }
}