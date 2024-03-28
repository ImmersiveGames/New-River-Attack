using System.Collections;
using ImmersiveGames.DebugManagers;
using UnityEngine;

namespace ImmersiveGames.ShopManagers.NavigationModes
{
    public class SmoothInfiniteNavigationMode : InfiniteNavigationMode
    {
        private const float SmoothTime = 0.2f; // Tempo para suavizar o movimento
        private bool _isMoving; // Flag para indicar se um movimento está em andamento
        private const float SmoothTransitionThreshold = 0.5f; // Threshold para aplicar suavização adicional
        private const float MaxSmoothTime = 0.2f; // Tempo máximo de suavização
        private const float Approximation = 0.2f; // Aproximação para determinar a suavização

        public override void MoveContent(RectTransform content, bool forward, MonoBehaviour monoBehaviour = null)
        {
            if (_isMoving)
            {
                DebugManager.LogWarning<SmoothInfiniteNavigationMode>("Uma movimentação já está em andamento. Aguarde até que termine.");
                return;
            }

            if (IsAtEdge(content, forward))
            {
                // Se estiver na borda, mova sem suavização para reiniciar o loop
                base.MoveContent(content, forward, monoBehaviour);
            }
            else
            {
                if (monoBehaviour != null) monoBehaviour.StartCoroutine(MoveSmoothly(content, forward));
            }
        }

        private IEnumerator MoveSmoothly(RectTransform content, bool forward)
        {
            _isMoving = true;
            DebugManager.Log<SmoothInfiniteNavigationMode>("Iniciando movimento suave.");

            var rect = content.anchoredPosition;
            var childCount = content.childCount;
            var moveAmount = content.rect.width / childCount;
            var targetX = CalculateTargetX(rect.x, moveAmount, forward);
            var minPosition = CalculateMinPosition(moveAmount, childCount);

            var velocity = 0f;
            var smoothTime = CalculateSmoothTime(rect.x, targetX);

            if (Mathf.Abs(rect.x - targetX) > SmoothTransitionThreshold)
            {
                smoothTime = Mathf.Min(smoothTime, MaxSmoothTime); // Limitar o tempo de suavização ao máximo
            }

            while (Mathf.Abs(rect.x - targetX) > SmoothTransitionThreshold)
            {
                rect.x = Mathf.SmoothDamp(rect.x, targetX, ref velocity, smoothTime);
                rect.x = Mathf.Clamp(rect.x, minPosition, 0);
                content.anchoredPosition = rect;
                yield return null;
            }

            rect.x = targetX;
            content.anchoredPosition = rect;

            _isMoving = false;
            DebugManager.Log<SmoothInfiniteNavigationMode>("Movimento suave concluído.");

            // Calcula o índice do item centralizado após o movimento
            var selectedIndex = CalculateSelectedItemIndex(content, forward);

            // Chama o método para atualizar o item selecionado
            UpdateSelectedItem(content, selectedIndex);
        }

        private float CalculateTargetX(float currentX, float moveAmount, bool forward)
        {
            return forward ? currentX - moveAmount : currentX + moveAmount;
        }

        private float CalculateMinPosition(float moveAmount, int childCount)
        {
            return -((childCount - 1) * moveAmount);
        }

        private float CalculateSmoothTime(float currentX, float targetX)
        {
            const float baseSmoothTime = SmoothTime; // Use o SmoothTime original como base
            return Mathf.Abs(currentX - targetX) > Approximation ? MaxSmoothTime : baseSmoothTime;
        }

        private bool IsAtEdge(RectTransform content, bool forward)
        {
            var rect = content.anchoredPosition;
            var childCount = content.childCount;
            var moveAmount = content.rect.width / childCount;
            var minPosition = CalculateMinPosition(moveAmount, childCount);
            return forward ? rect.x <= minPosition : rect.x >= 0;
        }
    }
}
