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
        public virtual void MoveContentToIndex(RectTransform content, int index)
        {
            // Implementação padrão: atualiza o índice, mas não faz nada visualmente
            SelectedItemIndex = index;
        }


        public virtual void MoveContent(RectTransform content, bool forward, MonoBehaviour monoBehaviour = null)
        {
            var rect = content.anchoredPosition;
            var childCount = content.childCount;
            if (childCount <= 0)
            {
                DebugManager.LogError<FiniteNavigationMode>($"Não há filhos criados");
                return;
            }

            // Calcula o próximo índice
            int nextIndex = forward ? SelectedItemIndex + 1 : SelectedItemIndex - 1;
            nextIndex = Mathf.Clamp(nextIndex, 0, childCount - 1);

            // Verifica se o próximo item é ativo e visível
            while (!content.GetChild(nextIndex).gameObject.activeSelf)
            {
                nextIndex = forward ? nextIndex + 1 : nextIndex - 1;

                if (nextIndex < 0 || nextIndex >= childCount)
                {
                    // Se sair dos limites, mantém o índice atual
                    nextIndex = SelectedItemIndex;
                    break;
                }
            }

            var visibleChild = content.GetChild(nextIndex);
            var moveAmount = visibleChild.GetComponent<RectTransform>().rect.width;

            const int maxPosition = 0;
            var minPosition = -((childCount - 1) * moveAmount);

            // Movimenta o conteúdo para a nova posição
            if (forward)
            {
                if (rect.x - moveAmount >= minPosition)
                {
                    rect.x -= moveAmount;
                    SelectedItemIndex = nextIndex;
                }
            }
            else
            {
                if (rect.x + moveAmount <= maxPosition)
                {
                    rect.x += moveAmount;
                    SelectedItemIndex = nextIndex;
                }
            }

            content.anchoredPosition = rect;
            UpdateSelectedItem(content, SelectedItemIndex);
        }


        /*private static int GetVisibleChildIndex(RectTransform content)
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
        }*/

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
