using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Collections;
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
        [SerializeField] AudioEventSample clickSound;
        protected int lastIndex; 

        #region UNITYMETHODS
        protected virtual void Awake()
        {
            SetLocalization();
            //Debug.Log($"Tempos: {fadeInTime} , {fadeOutTime}");
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

        static void SetSelectGameObject(GameObject goButton)
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
        void SetLocalization()
        {
            if (gameSettings.startLocale == null)
                gameSettings.startLocale = LocalizationSettings.SelectedLocale;
            StartCoroutine(SetLocale(gameSettings.startLocale));
            //Debug.Log($"LocalName: {gameSettings.startLocale.Identifier.Code}");
        }

        IEnumerator SetLocale(Locale localActual)
        {
            yield return LocalizationSettings.InitializationOperation;
            LocalizationSettings.SelectedLocale = localActual;
            gameSettings.startLocale = LocalizationSettings.SelectedLocale;
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
