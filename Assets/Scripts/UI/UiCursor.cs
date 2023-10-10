using System;
using UnityEngine;
using UnityEngine.EventSystems;
namespace RiverAttack
{
    public class UiCursor : MonoBehaviour
    {
        [SerializeField] RectTransform arrowLeft, arrowRight;

        GameObject m_ButtonActive;

        void OnEnable()
        {
            m_ButtonActive = EventSystem.current.currentSelectedGameObject;
        }

        void Update()
        {
            if (EventSystem.current.currentSelectedGameObject == null && m_ButtonActive != null)
            {
                EventSystem.current.SetSelectedGameObject(m_ButtonActive);
            }
            if (m_ButtonActive == EventSystem.current.currentSelectedGameObject)
                return;
            m_ButtonActive = EventSystem.current.currentSelectedGameObject;
            SetCursor(m_ButtonActive.GetComponent<RectTransform>());
        }
        void SetCursor(RectTransform reference)
        {
            var cursor = GetComponent<RectTransform>();
            cursor.position = reference.position;
            //cursor.pivot = reference.pivot;
            var anchoredPosition = reference.anchoredPosition;
            cursor.anchoredPosition = anchoredPosition;

            arrowRight.anchoredPosition = new Vector2(reference.anchoredPosition.x + (reference.rect.width/2) + (arrowRight.rect.width/2), arrowRight.anchoredPosition.y);
            arrowLeft.anchoredPosition = new Vector2(reference.anchoredPosition.x - (reference.rect.width/2) - (arrowLeft.rect.width/2), arrowLeft.anchoredPosition.y);
        }
    }
}
