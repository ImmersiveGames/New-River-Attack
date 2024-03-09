using System.Collections.Generic;
using UnityEngine;

namespace ImmersiveGames.MenuManagers.Abstracts
{
    public abstract class AbstractMenuManager : MonoBehaviour
    {
        public GameObject[] menus;

        private readonly Stack<int> _menuHistory = new Stack<int>();
        private int _currentMenuIndex;

        // Evento que será chamado quando um menu for aberto ou fechado
        public delegate void MenuChangeEvent(string exitingMenu, string enteringMenu);

        public static event MenuChangeEvent EventOnMenuChange;

        // Método abstrato para entrar no menu
        protected abstract void OnEnterMenu(GameObject menuGameObject);

        // Método abstrato para sair do menu
        protected abstract void OnExitMenu(GameObject menuGameObject);

        // Método para ativar um menu específico pelo índice
        public void ActivateMenu(int index)
        {
            if (index >= 0 && index < menus.Length)
            {
                // Desativar todos os menus
                foreach (var menu in menus)
                {
                    menu.SetActive(false);
                }

                // Adicionar o índice do menu ao histórico
                _menuHistory.Push(_currentMenuIndex);

                // Chamar o método para sair do menu atual
                OnExitMenu(menus[_currentMenuIndex]);

                // Atualizar o índice do menu atual
                _currentMenuIndex = index;

                // Ativar o menu desejado
                menus[index].SetActive(true);

                SetSelectGameObject(menus[index]);
                    
                // Chamar o método para entrar no novo menu
                OnEnterMenu(menus[index]);

                // Chamar o evento de troca de menu
                EventOnMenuChange?.Invoke(menus[_menuHistory.Peek()].name, menus[index].name);
            }
            else
            {
                Debug.LogError("Índice de menu inválido.");
            }
        }
        // Método para indicar qual o botão será o primeiro do menu
        private static void SetSelectGameObject(GameObject firstButton)
        {
            var eventSystemFirstSelect = firstButton.GetComponentInChildren<SystemEventFirstSelect>();
            if (eventSystemFirstSelect != null)
            {
                eventSystemFirstSelect.Init();
            }
        }

        // Método para voltar ao menu anterior
        public void GoBack()
        {
            if (_menuHistory.Count <= 0) return;
            var previousMenuIndex = _menuHistory.Pop();
            AudioManager.PlayMouseClick();
            ActivateMenu(previousMenuIndex);

            // Se quiser realizar alguma ação específica quando o botão de voltar é pressionado, pode fazê-lo aqui
            // Exemplo: Debug.Log("Botão de voltar pressionado para o menu anterior.");
        }

        public void ButtonExit()
        {
            AudioManager.PlayMouseClick();
            Application.Quit();
        }
    }
}
