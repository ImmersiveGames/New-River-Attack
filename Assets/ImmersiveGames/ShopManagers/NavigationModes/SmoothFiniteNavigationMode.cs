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
                // Interrompe a coroutine anterior antes de iniciar uma nova
                if (monoBehaviour != null) monoBehaviour.StopCoroutine(nameof(MoveToPosition));
            }

            if (monoBehaviour != null)
                monoBehaviour.StartCoroutine(MoveToPosition(content, forward, content.childCount));
        }
        public override void MoveContentToIndex(RectTransform content, int index)
        {
            var layoutGroup = content.GetComponent<HorizontalLayoutGroup>();  // Obtendo o layout aqui (esse GetComponent é seguro para o HorizontalLayoutGroup)
            if (layoutGroup != null)
            {
                _moveCount = index;
                MoveToSpecificPosition(content, index, layoutGroup);
            }
            else
            {
                DebugManager.LogError<SmoothFiniteNavigationMode>("HorizontalLayoutGroup não encontrado no content.");
            }
        }

        private void MoveToSpecificPosition(RectTransform content, int targetIndex, HorizontalLayoutGroup layoutGroup)
        {
            var moveAmount = layoutGroup.spacing;  // Usa o espaçamento do layout
            var targetX = -targetIndex * moveAmount;

            // Ajusta a posição suavemente
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
