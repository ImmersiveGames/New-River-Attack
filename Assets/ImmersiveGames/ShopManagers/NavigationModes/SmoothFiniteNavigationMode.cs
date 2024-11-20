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
            if (_isMoving)
            {
                DebugManager.Log<SmoothFiniteNavigationMode>("Movimento já em andamento. Ignorado.");
                return; // Previne nova movimentação durante o processo atual
            }

            if (monoBehaviour == null) return;

            // Verifica limites para evitar movimentos fora do intervalo
            if ((!forward && _moveCount <= 0) || (forward && _moveCount >= content.childCount - 1))
            {
                DebugManager.Log<SmoothFiniteNavigationMode>("Movimento nos limites. Ignorado.");
                return;
            }

            monoBehaviour.StartCoroutine(MoveToPosition(content, forward, content.childCount));
        }

        public override void MoveContentToIndex(RectTransform content, int index)
        {
            // Sincronização do índice
            _moveCount = Mathf.Clamp(index, 0, content.childCount - 1);
            SelectedItemIndex = _moveCount; // Garante que ambos estão sincronizados

            var layoutGroup = content.GetComponent<HorizontalLayoutGroup>();
            if (layoutGroup != null)
            {
                MoveToSpecificPosition(content, _moveCount, layoutGroup);
            }
            else
            {
                DebugManager.LogError<SmoothFiniteNavigationMode>("HorizontalLayoutGroup não encontrado no content.");
            }
        }

        private void MoveToSpecificPosition(RectTransform content, int targetIndex, HorizontalLayoutGroup layoutGroup)
        {
            var itemWidth = content.GetChild(0).GetComponent<RectTransform>().rect.width;
            var moveAmount = itemWidth + layoutGroup.spacing;

            var targetX = -targetIndex * moveAmount;

            // Ajusta a posição imediatamente
            content.anchoredPosition = new Vector2(targetX, content.anchoredPosition.y);
            DebugManager.Log<SmoothFiniteNavigationMode>($"Movido para a posição do item de índice: {targetIndex}");
        }

        private IEnumerator MoveToPosition(RectTransform content, bool forward, int totalItems)
        {
            _isMoving = true;
            DebugManager.Log<SmoothFiniteNavigationMode>("Iniciando movimento.");

            _moveCount += forward ? 1 : -1;
            _moveCount = Mathf.Clamp(_moveCount, 0, totalItems - 1);

            // Sincroniza o índice após o movimento
            SelectedItemIndex = _moveCount;
            DebugManager.Log<SmoothFiniteNavigationMode>($"Índice sincronizado após movimento: {SelectedItemIndex}");

            var itemWidth = content.GetChild(0).GetComponent<RectTransform>().rect.width;
            var moveAmount = itemWidth + content.GetComponent<HorizontalLayoutGroup>().spacing;

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
