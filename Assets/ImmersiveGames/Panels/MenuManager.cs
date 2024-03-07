using ImmersiveGames.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace ImmersiveGames.Panels
{
    public class MenuManager : Singleton<MenuManager>
    {
        [Header("Global Interactivity Settings")]
        [SerializeField] private bool isGlobalSelectable = true;
        [SerializeField] private bool isGlobalNavigable = true;

        public UnityEvent onMenuSelected = new UnityEvent();
        public UnityEvent onMenuNavigated = new UnityEvent();

        private PanelBase _currentSelectedMenu;

        public void InteractWithMenu(PanelBase menu)
        {
            if (menu == null)
            {
                Debug.LogWarning("InteractWithMenu: Menu is null.");
                return;
            }

            if (isGlobalSelectable && menu.isSelectable)
            {
                if (_currentSelectedMenu != menu)
                {
                    onMenuSelected?.Invoke();
                    Debug.Log("Menu selected!");
                    _currentSelectedMenu = menu;
                }
            }

            if (isGlobalNavigable && menu.isNavigable)
            {
                onMenuNavigated?.Invoke();
                Debug.Log("Menu navigable!");
            }
        }

        public PanelBase GetCurrentSelectedMenu()
        {
            return _currentSelectedMenu;
        }

        // Other methods and logic for MenuManager...
    }
}