using UnityEngine;
using System.Collections;
using ImmersiveGames.DebugManagers;

namespace ImmersiveGames.ShopManagers.NavigationModes
{
    public class SmoothFiniteNavigationMode : FiniteNavigationMode
    {
        private const float SmoothTime = 0.1f;
        private bool _isMoving;
        private int _selectedItemIndex;
        private const float Approximation = 0.3f; // Aproximação para determinar a suavização

        public override void MoveContent(RectTransform content, bool forward, MonoBehaviour monoBehaviour = null)
        {
            switch (_isMoving)
            {
                case false when monoBehaviour != null:
                    monoBehaviour.StartCoroutine(MoveToPosition(content, forward, content.childCount));
                    break;
                case true:
                    DebugManager.LogWarning<SmoothFiniteNavigationMode>("Uma movimentação já está em andamento. Aguarde até que termine.");
                    break;
            }
        }

        private IEnumerator MoveToPosition(RectTransform content, bool forward, int totalItems)
        {
            _isMoving = true;
            DebugManager.Log<SmoothFiniteNavigationMode>("Iniciando movimento.");
            var rect = content.anchoredPosition;
            var childCount = totalItems;
            var moveAmount = content.rect.width / childCount;
            const float maxPosition = 0f;
            var minPosition = -((childCount - 1) * moveAmount);

            var targetX = forward ? rect.x - moveAmount : rect.x + moveAmount;
            targetX = forward switch
            {
                true when targetX < minPosition => minPosition,
                false when targetX > maxPosition => maxPosition,
                _ => targetX
            };

            var velocity = 0f;
            while (Mathf.Abs(rect.x - targetX) > Approximation)
            {
                rect.x = Mathf.SmoothDamp(rect.x, targetX, ref velocity, SmoothTime);
                rect.x = Mathf.Clamp(rect.x, minPosition, maxPosition);
                content.anchoredPosition = rect;
                yield return null;
            }

            rect.x = targetX;
            content.anchoredPosition = rect;

            _isMoving = false;
            DebugManager.Log<SmoothFiniteNavigationMode>("Movimento concluído.");

            var selectedIndex = CalculateSelectedItemIndex(content, forward);
            UpdateSelectedItem(content, selectedIndex);
        }
    }
}
