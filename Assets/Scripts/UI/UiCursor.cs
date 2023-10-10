using UnityEngine;
namespace RiverAttack
{
    public class UiCursor : MonoBehaviour
    {
        public void SetCursor(RectTransform reference, float offset)
        {
            var cursor = GetComponent<RectTransform>();

            float newOffset = offset + ((cursor.rect.width / 2) - (reference.rect.width/2)) ;
            Debug.Log($"Ancora: {reference.anchoredPosition.x}, Tamanho: {reference.rect.width}, Offset: {newOffset}");
            cursor.anchoredPosition = new Vector2((reference.anchoredPosition.x - reference.rect.width) - newOffset, reference.anchoredPosition.y);
        }
    }
}
