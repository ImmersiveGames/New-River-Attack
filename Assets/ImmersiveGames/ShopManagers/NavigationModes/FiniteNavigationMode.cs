using ImmersiveGames.DebugManagers;
using ImmersiveGames.ShopManagers.Interfaces;
using NewRiverAttack.ShoppingSystems.SimpleShopping;
using NewRiverAttack.ShoppingSystems.SimpleShopping.Abstracts;
using UnityEngine;
using UnityEngine.EventSystems;

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
            var nextIndex = forward ? SelectedItemIndex + 1 : SelectedItemIndex - 1;
            nextIndex = Mathf.Clamp(nextIndex, 0, childCount - 1);

            // Verifica se o próximo item é ativo e visível
            while (!content.GetChild(nextIndex).gameObject.activeSelf)
            {
                nextIndex = forward ? nextIndex + 1 : nextIndex - 1;

                if (nextIndex >= 0 && nextIndex < childCount) continue;
                // Se sair dos limites, mantém o índice atual
                nextIndex = SelectedItemIndex;
                break;
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

        public void UpdateSelectedItem(RectTransform content, int selectedIndex)
        {
            if (content.childCount <= 0) return;

            // Obtém o item selecionado
            var selectedItem = content.GetChild(selectedIndex).GetComponent<ShopProductSettings>();

            if (selectedItem == null) return;
            // Verifique se o item é usável ou comprável e destaque o botão correto
            var shopProductSimpleSkins = selectedItem as ShopProductSimpleSkins;
            if (shopProductSimpleSkins == null) return;
            if (shopProductSimpleSkins.buttonUse.gameObject.activeSelf)
            {
                // Foca no botão de usar se estiver ativo
                EventSystem.current.SetSelectedGameObject(shopProductSimpleSkins.buttonUse.gameObject);
            }
            else if (shopProductSimpleSkins.buttonBuy.gameObject.activeSelf)
            {
                // Foca no botão de comprar se estiver ativo
                EventSystem.current.SetSelectedGameObject(shopProductSimpleSkins.buttonBuy.gameObject);
            }
        }

    }
}
