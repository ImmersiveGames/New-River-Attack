using UnityEngine;
using UnityEngine.EventSystems;

namespace ImmersiveGames.MenuManagers.UI
{
    public class UiPanelCursor : MonoBehaviour
    {
        [SerializeField] private RectTransform arrowLeft;
        private GameObject _currentActiveButton;
        
        private bool _isLoadMenu;
        

        private void Start()
        {
            _currentActiveButton = EventSystem.current.currentSelectedGameObject;
            if (_currentActiveButton != null)
                SetCursor(_currentActiveButton.GetComponent<RectTransform>());
            _isLoadMenu = true;
        }

        private void Update()
        {
            if (EventSystem.current.currentSelectedGameObject == null && _currentActiveButton != null)
            {
                EventSystem.current.SetSelectedGameObject(_currentActiveButton);
            }

            if (_currentActiveButton == EventSystem.current.currentSelectedGameObject) return;
            _currentActiveButton = EventSystem.current.currentSelectedGameObject;
            SetCursor(_currentActiveButton.GetComponent<RectTransform>());
        }

        private void OnDisable()
        {
            _isLoadMenu = false;
        }

        public GameObject GetCurrentActiveButton => _currentActiveButton;
        public GameObject SetCurrentActiveButton => _currentActiveButton = null;

        private void SetCursor(RectTransform reference)
        {
            if (reference == null) return;
            if (AudioManager.instance && _isLoadMenu)
            {
                AudioManager.PlayOneShot(EnumSfxSound.SfxMouseOver.ToString());
            }
            var cursor = GetComponent<RectTransform>();
            cursor.position = reference.position;
            var anchoredPosition = reference.anchoredPosition;
            cursor.anchoredPosition = anchoredPosition;
            arrowLeft.anchoredPosition = new Vector2(anchoredPosition.x - (reference.rect.width / 2) - (arrowLeft.rect.width / 2), arrowLeft.anchoredPosition.y);
        }
    }
}