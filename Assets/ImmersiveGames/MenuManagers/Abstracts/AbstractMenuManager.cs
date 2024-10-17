using System.Collections;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.MenuManagers.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ImmersiveGames.MenuManagers.Abstracts
{
    public abstract class AbstractMenuManager : MonoBehaviour
    {
        private PanelsMenuReference[] _menus;
        private int _currentMenuIndex;
        private int _previousMenuIndex = -1; // Armazena o índice do menu anterior

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
                // Armazena o índice atual como o anterior
                _previousMenuIndex = _currentMenuIndex;

                // Desativa todos os menus
                foreach (var menu in _menus)
                {
                    menu.menuGameObject.SetActive(false);
                    if (menu.virtualCameraBase != null)
                    {
                        menu.virtualCameraBase.Priority = 0;
                        menu.virtualCameraBase.gameObject.SetActive(false);
                    }
                }

                // Atualiza o índice do menu atual
                _currentMenuIndex = index;
                _menus[index].menuGameObject.SetActive(true);

                // Ativa a câmera virtual, se existir
                if (_menus[index].virtualCameraBase != null)
                {
                    _menus[index].virtualCameraBase.Priority = 10;
                    _menus[index].virtualCameraBase.gameObject.SetActive(true);
                }

                // Limpa qualquer seleção atual no EventSystem
                EventSystem.current.SetSelectedGameObject(null);

                // Inicia uma corrotina para selecionar o botão após ativação completa
                StartCoroutine(SelectMenuButtonWithDelay(_menus[index].firstSelect));

                OnEnterMenu(_menus[index]);

                // Dispara o evento de mudança de menu
                EventOnMenuChange?.Invoke(_menus[_previousMenuIndex].menuGameObject.name, _menus[index].menuGameObject.name);
            }
            else
            {
                DebugManager.LogError<AbstractMenuManager>("Índice de menu inválido.");
            }
        }
        private IEnumerator SelectMenuButtonWithDelay(GameObject firstSelectObject)
        {
            // Espera um frame para garantir que o menu está completamente ativo
            yield return null;

            // Garante que o layout foi atualizado corretamente
            Canvas.ForceUpdateCanvases();

            // Seleciona o botão inicial
            if (firstSelectObject != null)
            {
                EventSystem.current.SetSelectedGameObject(firstSelectObject);
            }
        }



        private void SetSelectGameObject(GameObject firstSelectObject)
        {
            if (firstSelectObject == null) return;

            var eventSystem = EventSystem.current;
            var cursor = _uiPanelCursor?.GetCurrentActiveButton;

            // Se houver um cursor ativo, usá-lo
            if (cursor != null)
            {
                firstSelectObject = cursor;
            }

            // Limpa a seleção atual no EventSystem
            eventSystem.SetSelectedGameObject(null);

            // Atraso para garantir que o GameObject esteja ativo
            StartCoroutine(SelectAfterDelay(firstSelectObject));
        }

        private IEnumerator SelectAfterDelay(GameObject firstSelectObject)
        {
            // Aguarda um frame para garantir que o menu/botão esteja ativo
            yield return null; 

            EventSystem.current.SetSelectedGameObject(firstSelectObject);
        }


        public void HistoryGoBack()
        {
            GoBack();
        }

        public virtual void GoBack()
        {
            // Verifica se há um menu anterior definido
            if (_previousMenuIndex >= 0)
            {
                ActivateMenu(_previousMenuIndex); // Ativa o menu anterior diretamente
            }
        }

        protected void DisableOnPress(PanelsMenuReference panelsMenuGameObject)
        {
            var clickedObject = EventSystem.current?.currentSelectedGameObject;
            if (clickedObject == null) return;

            var actualButton = clickedObject.GetComponent<Button>();
            DebugManager.Log<AbstractMenuManager>("Botão acionado por: " + actualButton.name);

            var allButtons = panelsMenuGameObject.menuGameObject.GetComponentsInChildren<Button>();
            foreach (var button in allButtons)
            {
                if (button == actualButton) continue;
                button.interactable = false;
            }
        }

        protected PanelsMenuReference GetCurrentMenu => _menus[_currentMenuIndex];

        public void ButtonExit()
        {
            AudioManager.instance.PlayMouseClick();
            Application.Quit();
        }
    }
}
