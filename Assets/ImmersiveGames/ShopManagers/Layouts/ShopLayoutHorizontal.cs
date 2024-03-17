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

            horizontalLayoutGroup.padding.right = horizontalLayoutGroup.padding.left = (int)productWidth / 2;
            horizontalLayoutGroup.spacing = productWidth; // Espaço igual ao tamanho do produto
            content.sizeDelta = new Vector2((itemCount - 1) * productWidth + productWidth, content.sizeDelta.y); // Largura do conteúdo ajustada para caber todos os produtos

            horizontalLayoutGroup.childControlHeight = true;
            horizontalLayoutGroup.childControlWidth = true;
            horizontalLayoutGroup.childScaleHeight = true;
            horizontalLayoutGroup.childScaleWidth = true;
            horizontalLayoutGroup.childForceExpandHeight = true;
            horizontalLayoutGroup.childForceExpandWidth = false;
            
        }
    }
}