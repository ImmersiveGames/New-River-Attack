using UnityEngine;
using System.Collections;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.ShopManagers.Interfaces;

namespace ImmersiveGames.ShopManagers.NavigationModes
{
    public class SmoothFiniteNavigationMode : FiniteNavigationMode
    {
        private const float SmoothTime = 0.2f;
        private bool _isMoving;
        private int _selectedItemIndex;

        public override void MoveContent(RectTransform content, bool forward, MonoBehaviour monoBehaviour = null)
        {
            switch (_isMoving)
            {
                case false when monoBehaviour != null:
                    monoBehaviour.StartCoroutine(MoveToPosition(content, forward, content.childCount));
                    break;
                case true:
                    DebugManager.LogWarning("Uma movimentação já está em andamento. Aguarde até que termine.");
                    break;
            }
        }

        private IEnumerator MoveToPosition(RectTransform content, bool forward, int totalItems)
        {
            _isMoving = true;
            DebugManager.Log("Iniciando movimento.");
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
            while (Mathf.Abs(rect.x - targetX) > 0.01f)
            {
                rect.x = Mathf.SmoothDamp(rect.x, targetX, ref velocity, SmoothTime);
                rect.x = Mathf.Clamp(rect.x, minPosition, maxPosition);
                content.anchoredPosition = rect;
                yield return null;
            }

            rect.x = targetX;
            content.anchoredPosition = rect;

            _isMoving = false;
            DebugManager.Log("Movimento concluído.");

            var selectedIndex = CalculateSelectedItemIndex(content, forward);
            UpdateSelectedItem(selectedIndex);
        }
    }
}
