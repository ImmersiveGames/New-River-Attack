using System;
using System.Collections.Generic;
using Cinemachine;
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

        // Evento que será chamado quando um menu for aberto ou fechado
        public delegate void MenuChangeEvent(string exitingMenu, string enteringMenu);

        public static event MenuChangeEvent EventOnMenuChange;

        protected void SetMenu(PanelsMenuReference[] menus)
        {
            _menus = menus;
        }

        // Método abstrato para entrar no menu
        protected abstract void OnEnterMenu(PanelsMenuReference panelsMenuGameObject);

        // Método abstrato para sair do menu
        protected abstract void OnExitMenu(PanelsMenuReference panelsMenuGameObject);

        // Método para ativar um menu específico pelo índice
        public void ActivateMenu(int index)
        {
            AudioManager.PlayMouseClick();
            if (index >= 0 && index < _menus.Length)
            {
                // Desativar todos os menus
                foreach (var menu in _menus)
                {
                    menu.menuGameObject.SetActive(false);
                    menu.virtualCameraBase.Priority = 0;
                    menu.virtualCameraBase.gameObject.SetActive(false);
                }
                // Adicionar o índice do menu ao histórico
                _menuHistory.Push(_currentMenuIndex);

                // Chamar o método para sair do menu atual
                OnExitMenu(_menus[_currentMenuIndex]);
                
                // Atualizar o índice do menu atual
                _currentMenuIndex = index;

                // Ativar o menu desejado
                _menus[index].menuGameObject.SetActive(true);
                _menus[index].virtualCameraBase.Priority = 10;
                _menus[index].virtualCameraBase.gameObject.SetActive(true);

                SetSelectGameObject(_menus[index].firstSelect);
                    
                // Chamar o método para entrar no novo menu
                OnEnterMenu(_menus[index]);

                // Chamar o evento de troca de menu
                EventOnMenuChange?.Invoke(_menus[_menuHistory.Peek()].menuGameObject.name, _menus[index].menuGameObject.name);
            }
            else
            {
                DebugManager.LogError("Índice de menu inválido.");
            }
        }
        // Método para indicar qual o botão será o primeiro do menu
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

        // Método para voltar ao menu anterior
        public void HistoryGoBack()
        {
            DebugManager.Log($"[Pilha] Size: {_menuHistory.Count}");
            if (_menuHistory.Count <= 1) return;
            var previousMenuIndex = _menuHistory.Pop();
            AudioManager.PlayMouseClick();
            var cursor = GetComponentInChildren<UiPanelCursor>()?.SetCurrentActiveButton;
            ActivateMenu(previousMenuIndex);
        }
        public void GoBack()
        {
            DebugManager.Log($"[Pilha] Size: {_menuHistory.Count}");
            if (_menuHistory.Count <= 1) return;

            // Remover o menu atual do histórico
            _menuHistory.Pop();

            // Obter o menu anterior sem removê-lo do histórico
            var previousMenuIndex = _menuHistory.Peek();

            AudioManager.PlayMouseClick();
            var cursor = GetComponentInChildren<UiPanelCursor>()?.SetCurrentActiveButton;
            ActivateMenu(previousMenuIndex);
        }

        public void ButtonExit()
        {
            AudioManager.PlayMouseClick();
            Application.Quit();
        }
    }
    [Serializable]
    public class PanelsMenuReference
    {
        public GameObject menuGameObject;
        public GameObject firstSelect;
        public float startTimelineAnimation;
        public CinemachineVirtualCameraBase virtualCameraBase;
    } 
}
