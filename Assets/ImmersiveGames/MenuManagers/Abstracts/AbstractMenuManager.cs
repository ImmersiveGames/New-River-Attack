using System.Collections.Generic;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.MenuManagers.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ImmersiveGames.MenuManagers.Abstracts
{
    public abstract class AbstractMenuManager : MonoBehaviour
    {
        private PanelsMenuReference[] _menus;
        private readonly Stack<int> _menuHistory = new Stack<int>();
        private int _currentMenuIndex;

        private UiPanelCursor _uiPanelCursor;

        public delegate void MenuChangeEvent(string exitingMenu, string enteringMenu);

        public static event MenuChangeEvent EventOnMenuChange;

        protected void SetMenu(PanelsMenuReference[] menus)
        {
            _menus = menus;
        }

        protected abstract void OnEnterMenu(PanelsMenuReference panelsMenuGameObject);

        protected abstract void OnExitMenu(PanelsMenuReference panelsMenuGameObject);

        private void Awake()
        {
            _uiPanelCursor = GetComponentInChildren<UiPanelCursor>();
        }

        public void ActivateMenu(int index)
        {
            if (index >= 0 && index < _menus.Length)
            {
                foreach (var menu in _menus)
                {
                    menu.menuGameObject.SetActive(false);
                    if (!menu.virtualCameraBase) continue;
                    menu.virtualCameraBase.Priority = 0;
                    menu.virtualCameraBase.gameObject.SetActive(false);
                }

                _menuHistory.Push(_currentMenuIndex);
                OnExitMenu(_menus[_currentMenuIndex]);

                _currentMenuIndex = index;
                _menus[index].menuGameObject.SetActive(true);
                if (_menus[index].virtualCameraBase)
                {
                    _menus[index].virtualCameraBase.Priority = 10;
                    _menus[index].virtualCameraBase.gameObject.SetActive(true);
                }

                SetSelectGameObject(_menus[index].firstSelect);
                OnEnterMenu(_menus[index]);

                EventOnMenuChange?.Invoke(_menus[_menuHistory.Peek()].menuGameObject.name, _menus[index].menuGameObject.name);
            }
            else
            {
                DebugManager.LogError<AbstractMenuManager>("Índice de menu inválido.");
            }
        }

        private void SetSelectGameObject(GameObject firstSelectObject)
        {
            if (!firstSelectObject) return;
            var eventSystem = EventSystem.current;
            var cursor = _uiPanelCursor?.GetCurrentActiveButton;
            if (cursor != null)
            {
                firstSelectObject = cursor;
            }
            eventSystem.SetSelectedGameObject(firstSelectObject);
        }

        public void HistoryGoBack()
        {
            NavigateBack(true);
        }

        public virtual void GoBack()
        {
            NavigateBack(false);
        }

        private void NavigateBack(bool removeCurrent)
        {
            DebugManager.Log<AbstractMenuManager>($"[Pilha] Size: {_menuHistory.Count}");
            if (_menuHistory.Count <= 1) return;
            
            var previousMenuIndex = _menuHistory.Peek();
            AudioManager.instance.PlayMouseClick();
            _uiPanelCursor?.ClearCurrentActiveButton(); // Corrigido para limpar o botão ativo
            ActivateMenu(previousMenuIndex);

            if (removeCurrent)
            {
                _menuHistory.Pop();
            }
        }

        public void ButtonExit()
        {
            AudioManager.instance.PlayMouseClick();
            Application.Quit();
        }
    }
}
