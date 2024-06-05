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

        public delegate void MenuChangeEvent(string exitingMenu, string enteringMenu);

        public static event MenuChangeEvent EventOnMenuChange;

        protected void SetMenu(PanelsMenuReference[] menus)
        {
            _menus = menus;
        }

        protected abstract void OnEnterMenu(PanelsMenuReference panelsMenuGameObject);

        protected abstract void OnExitMenu(PanelsMenuReference panelsMenuGameObject);

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
            var cursor = GetComponentInChildren<UiPanelCursor>()?.GetCurrentActiveButton;
            if (cursor != null)
            {
                firstSelectObject = cursor;
            }
            eventSystem.SetSelectedGameObject(firstSelectObject);
        }

        public void HistoryGoBack()
        {
            DebugManager.Log<AbstractMenuManager>($"[Pilha] Size: {_menuHistory.Count}");
            if (_menuHistory.Count <= 1) return;
            var previousMenuIndex = _menuHistory.Pop();
            AudioManager.PlayMouseClick();
            var cursor = GetComponentInChildren<UiPanelCursor>()?.SetCurrentActiveButton;
            ActivateMenu(previousMenuIndex);
        }
        public void GoBack()
        {
            DebugManager.Log<AbstractMenuManager>($" Size: {_menuHistory.Count}");
            if (_menuHistory.Count <= 1) return;

            // Obter o índice do menu anterior sem removê-lo da pilha
            var previousMenuIndex = _menuHistory.Peek();

            AudioManager.PlayMouseClick();
            var cursor = GetComponentInChildren<UiPanelCursor>()?.SetCurrentActiveButton;
            ActivateMenu(previousMenuIndex);

            // Remover o menu atual da pilha após ativá-lo novamente
            _menuHistory.Pop();
        }

        public void ButtonExit()
        {
            AudioManager.PlayMouseClick();
            Application.Quit();
        }
    }
   
}