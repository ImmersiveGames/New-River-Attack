using UnityEngine;
using UnityEngine.EventSystems;
namespace RiverAttack
{
    public class UiCursor : MonoBehaviour
    {
        [SerializeField] RectTransform arrowLeft, arrowRight;
        GameObject m_ButtonActive;
        public AudioEventSample onOverCursorSfx;

        void Update()
        {
            if (EventSystem.current.currentSelectedGameObject == null && m_ButtonActive != null)
            {
                EventSystem.current.SetSelectedGameObject(m_ButtonActive);
            }
            if (m_ButtonActive == EventSystem.current.currentSelectedGameObject)
                return;
            if (m_ButtonActive != null)
                GameAudioManager.instance.PlaySfx(onOverCursorSfx);
            m_ButtonActive = EventSystem.current.currentSelectedGameObject;
            SetCursor(m_ButtonActive.GetComponent<RectTransform>());
        }
        void SetCursor(RectTransform reference)
        {
            var cursor = GetComponent<RectTransform>();
            cursor.position = reference.position;
            var position = reference.anchoredPosition;
            cursor.anchoredPosition = position;

            arrowRight.anchoredPosition = new Vector2(position.x + (reference.rect.width / 2) + (arrowRight.rect.width / 2), arrowRight.anchoredPosition.y);
            arrowLeft.anchoredPosition = new Vector2(reference.anchoredPosition.x - (reference.rect.width / 2) - (arrowLeft.rect.width / 2), arrowLeft.anchoredPosition.y);
        }
    }
}
