using UnityEngine;
using UnityEngine.EventSystems;
namespace RiverAttack
{
    public class EventSystemFirstSelect : MonoBehaviour
    {
        public bool haveCursor;

        EventSystem m_EventSystem;
        void Awake()
        {
            m_EventSystem = EventSystem.current;
        }
        public void Init()
        {
            m_EventSystem.SetSelectedGameObject(gameObject);
        }

        /*public void SetCursor(ref RectTransform cursor, float offset)
        {
            if (haveCursor)
            {
                cursor.gameObject.SetActive(true);
                var referenceRectTransform = gameObject.GetComponent<RectTransform>();
                float newOffset = offset + (cursor.rect.width / 2) - (referenceRectTransform.rect.width/2) ;
                Debug.Log($"Ancora: {referenceRectTransform.anchoredPosition.x}, Tamanho: {referenceRectTransform.rect.width}, Offset: {newOffset}");
                cursor.anchoredPosition = new Vector2((referenceRectTransform.anchoredPosition.x - referenceRectTransform.rect.width) - newOffset, referenceRectTransform.anchoredPosition.y);
            }
            else
            {
                cursor.gameObject.SetActive(false);
            }
            
        }*/
    }
}