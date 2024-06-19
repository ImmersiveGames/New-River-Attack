using ImmersiveGames.ShopManagers.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace ImmersiveGames.ShopManagers.Layouts
{
    public class ShopLayoutHorizontal : IShopLayout
    {
        public void ConfigureLayout(RectTransform content, int itemCount, GameObject prefabItemShop)
        {
            var horizontalLayoutGroup = content.GetComponent<HorizontalLayoutGroup>();
            if (horizontalLayoutGroup == null)
            {
                horizontalLayoutGroup = content.gameObject.AddComponent<HorizontalLayoutGroup>();
            }

            var productWidth = 0f;

            // Obtém o RectTransform do prefab
            var prefabRectTransform = prefabItemShop.GetComponent<RectTransform>();
            if (prefabRectTransform != null)
            {
                productWidth = prefabRectTransform.rect.width;
            }
            else
            {
                Debug.LogError("O prefab não possui um RectTransform.");
            }

            horizontalLayoutGroup.padding.left = horizontalLayoutGroup.padding.right = 0;
            horizontalLayoutGroup.spacing = productWidth; // Espaço igual ao tamanho do produto
            content.sizeDelta = new Vector2(itemCount * productWidth, content.sizeDelta.y); // Largura do conteúdo ajustada para caber todos os produtos

            horizontalLayoutGroup.childControlHeight = true;
            horizontalLayoutGroup.childControlWidth = true;
            horizontalLayoutGroup.childScaleHeight = false;
            horizontalLayoutGroup.childScaleWidth = false;
            horizontalLayoutGroup.childForceExpandHeight = true;
            horizontalLayoutGroup.childForceExpandWidth = false;
        }

        public void ResetContentPosition(RectTransform content)
        {
            if (content.childCount == 0) return;

            // Define a posição inicial do conteúdo
            content.anchoredPosition = new Vector2(0, content.anchoredPosition.y);
        }
    }
}