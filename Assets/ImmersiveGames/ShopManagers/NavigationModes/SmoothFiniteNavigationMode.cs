using UnityEngine;
using System.Collections;
using ImmersiveGames.DebugManagers;
using UnityEngine.UI;

namespace ImmersiveGames.ShopManagers.NavigationModes
{
    public class SmoothFiniteNavigationMode : FiniteNavigationMode
    {
        private const float SmoothTime = 0.1f;
        private bool _isMoving;
        private const float Approximation = 0.3f;
        private int _moveCount;

        public override void MoveContent(RectTransform content, bool forward, MonoBehaviour monoBehaviour = null)
        {
            if (!_isMoving && monoBehaviour != null)
            {
                monoBehaviour.StartCoroutine(MoveToPosition(content, forward, content.childCount));
            }
            else
            {
                DebugManager.LogWarning<SmoothFiniteNavigationMode>("Uma movimentação já está em andamento. Aguarde até que termine.");
            }
        }
        public void MoveContentToIndex(RectTransform content, int index)
        {
            if (!_isMoving)
            {
                _moveCount = index;
                MoveToSpecificPosition(content, index);
            }
        }

        private void MoveToSpecificPosition(RectTransform content, int targetIndex)
        {
            var moveAmount = content.GetComponent<HorizontalLayoutGroup>().spacing;
            var targetX = -targetIndex * moveAmount;

            // Suaviza a transição
            content.anchoredPosition = new Vector2(targetX, content.anchoredPosition.y);
            DebugManager.Log<SmoothFiniteNavigationMode>($"Movido para a posição do item de índice: {targetIndex}");
        }

        private IEnumerator MoveToPosition(RectTransform content, bool forward, int totalItems)
        {
            _isMoving = true;
            DebugManager.Log<SmoothFiniteNavigationMode>("Iniciando movimento.");

            _moveCount += forward ? 1 : -1;
            _moveCount = Mathf.Clamp(_moveCount, 0, totalItems - 1);

            SelectedItemIndex = _moveCount;
            DebugManager.Log<SmoothFiniteNavigationMode>($"Índice selecionado: {SelectedItemIndex}");

            var moveAmount = content.GetComponent<HorizontalLayoutGroup>().spacing;
            var rect = content.anchoredPosition;
            var targetX = forward ? rect.x - moveAmount : rect.x + moveAmount;
            targetX = Mathf.Clamp(targetX, -((totalItems - 1) * moveAmount), 0f);

            var velocity = 0f;
            while (Mathf.Abs(rect.x - targetX) > Approximation)
            {
                rect.x = Mathf.SmoothDamp(rect.x, targetX, ref velocity, SmoothTime);
                content.anchoredPosition = rect;
                yield return null;
            }

            rect.x = targetX;
            content.anchoredPosition = rect;

            _isMoving = false;
            DebugManager.Log<SmoothFiniteNavigationMode>("Movimento concluído.");

            UpdateSelectedItem(content, SelectedItemIndex);
        }
    }
}
