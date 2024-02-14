using UnityEngine.Localization.Settings;
using UnityEngine;

namespace RiverAttack
{
    [RequireComponent(typeof(AudioSource))]
    public class PanelBase : MonoBehaviour
    {
        [Header("Menus")]
        [SerializeField] protected Transform menuInitial;
        [SerializeField] protected Transform[] menuPrincipal;
        [SerializeField] internal GameSettings gameSettings;        

        [Header("Menu SFX")]
        [SerializeField]
        private AudioEventSample clickSound;
        protected int lastIndex; 

        #region UNITYMETHODS
        protected virtual void Awake()
        {
            if(gameSettings.startLocale != null)
                LocalizationSettings.SelectedLocale = gameSettings.startLocale;
        }
  #endregion
        
        protected virtual void SetInternalMenu(int indexStart = 0)
        {
            if (menuPrincipal.Length < 1) return;

            for (int i = 0; i < menuPrincipal.Length; i++)
            {
                lastIndex = (menuPrincipal[i].gameObject.activeSelf) ? i : 0;
                menuPrincipal[i].gameObject.SetActive(false);
            }
            var selectPanel = menuPrincipal[indexStart].gameObject;
            selectPanel.SetActive(true);
            SetSelectGameObject(selectPanel);
        }

        private static void SetSelectGameObject(GameObject goButton)
        {
            var eventSystemFirstSelect = goButton.GetComponentInChildren<EventSystemFirstSelect>();
            if (eventSystemFirstSelect != null)
            {
                eventSystemFirstSelect.Init();
            }
        }

        internal void SetMenuPrincipal()
        {
            menuInitial.gameObject.SetActive(true);
            SetInternalMenu();
        }

        public void PlayClickSfx()
        {
            GameAudioManager.instance.PlaySfx(clickSound);
        }

        public void ButtonBack()
        {
            PlayClickSfx();
            SetInternalMenu(lastIndex);
        }
        public void ButtonIndexChange(int indexMenu)
        {
            PlayClickSfx();
            SetInternalMenu(indexMenu);
        }
    }
}
