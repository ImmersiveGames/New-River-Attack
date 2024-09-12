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

            if (monoBehaviour == null) return;
            switch (forward)
            {
                // Verifica se já estamos no primeiro ou no último item antes de iniciar a movimentação
                case false when _moveCount <= 0:
                    DebugManager.Log<SmoothFiniteNavigationMode>("Já no primeiro item, não é possível mover para trás.");
                    return;
                case true when _moveCount >= content.childCount - 1:
                    DebugManager.Log<SmoothFiniteNavigationMode>("Já no último item, não é possível mover para frente.");
                    return;
                default:
                    // Inicia a navegação
                    monoBehaviour.StartCoroutine(MoveToPosition(content, forward, content.childCount));
                    break;
            }
        }

        public override void MoveContentToIndex(RectTransform content, int index)
        {
            var layoutGroup = content.GetComponent<HorizontalLayoutGroup>();  // Obtendo o layout aqui (esse GetComponent é seguro para o HorizontalLayoutGroup)
            if (layoutGroup != null)
            {
                _moveCount = Mathf.Clamp(index, 0, content.childCount - 1);  // Garante que o índice está dentro do limite
                MoveToSpecificPosition(content, _moveCount, layoutGroup);
            }
            else
            {
                DebugManager.LogError<SmoothFiniteNavigationMode>("HorizontalLayoutGroup não encontrado no content.");
            }
        }

        private void MoveToSpecificPosition(RectTransform content, int targetIndex, HorizontalLayoutGroup layoutGroup)
        {
            var itemWidth = content.GetChild(0).GetComponent<RectTransform>().rect.width;  // Obtém a largura real do primeiro item
            var moveAmount = itemWidth + layoutGroup.spacing;  // Usa o tamanho real do item mais o espaçamento

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
            _moveCount = Mathf.Clamp(_moveCount, 0, totalItems - 1);  // Garante que o índice está dentro dos limites

            SelectedItemIndex = _moveCount;
            DebugManager.Log<SmoothFiniteNavigationMode>($"Índice selecionado: {SelectedItemIndex}");

            var itemWidth = content.GetChild(0).GetComponent<RectTransform>().rect.width;  // Obtém a largura real do primeiro item
            var moveAmount = itemWidth + content.GetComponent<HorizontalLayoutGroup>().spacing;  // Usa o tamanho real do item mais o espaçamento

            var rect = content.anchoredPosition;
            var targetX = forward ? rect.x - moveAmount : rect.x + moveAmount;
            targetX = Mathf.Clamp(targetX, -((totalItems - 1) * moveAmount), 0f);  // Garante que a posição não ultrapasse os limites

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
