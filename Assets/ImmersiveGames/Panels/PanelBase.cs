/*
using ImmersiveGames.ScriptableObjects;
using ImmersiveGames.Utils;
using UnityEngine.Localization.Settings;
using UnityEngine;

namespace ImmersiveGames
{
    [RequireComponent(typeof(AudioSource))]
    public class PanelBase : MonoBehaviour
    {
        [Header("Menus")]
        [SerializeField] private Transform menuInitial;
        [SerializeField] private Transform[] menuPrincipal;
        [SerializeField] private GameSettings gameSettings;

        [Header("Menu SFX")]
        [SerializeField] private AudioEvent clickSound;
        private int _lastIndex;

        #region UNITYMETHODS
        // Método chamado quando o objeto é instanciado
        protected virtual void Awake()
        {
            // Define o idioma inicial com base nas configurações do jogo
            LocalizationSettings.SelectedLocale = gameSettings.startLocale ? gameSettings.startLocale : LocalizationSettings.SelectedLocale;
        }
        #endregion

        // Define a visibilidade dos menus internos com base no índice de início
        protected virtual void SetInternalMenu(int indexStart = 0)
        {
            if (menuPrincipal.Length == 0) return;

            for (int i = 0; i < menuPrincipal.Length; i++)
            {
                bool isActive = i == indexStart;
                // Define a visibilidade do menu atual e inicializa o primeiro botão se estiver ativo
                menuPrincipal[i].gameObject.SetActive(isActive);
                if (isActive) SetSelectGameObject(menuPrincipal[i].gameObject);
            }
        }

        // Inicializa o primeiro botão do menu para receber seleção de evento do sistema
        private static void SetSelectGameObject(GameObject goButton)
        {
            var eventSystemFirstSelect = goButton.GetComponentInChildren<SystemEventFirstSelect>();
            if (eventSystemFirstSelect != null)
            {
                eventSystemFirstSelect.Init();
            }
        }

        // Ativa o menu principal e define sua visibilidade interna
        internal void SetMenuPrincipal()
        {
            menuInitial.gameObject.SetActive(true);
            SetInternalMenu();
        }

        // Reproduz o som de clique associado a interações de botão
        public void PlayClickSfx()
        {
            if (GameAudioManager.instance != null)
            {
                GameAudioManager.instance.PlaySfx(clickSound);
            }
        }

        // Volta para o menu anterior
        public void ButtonBack()
        {
            PlayClickSfx();
            SetInternalMenu(_lastIndex);
        }

        // Altera para o menu com o índice especificado
        public void ButtonIndexChange(int indexMenu)
        {
            PlayClickSfx();
            SetInternalMenu(indexMenu);
        }
    }
}
*/
