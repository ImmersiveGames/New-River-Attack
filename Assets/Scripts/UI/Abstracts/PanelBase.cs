using UnityEngine.SceneManagement;
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
        [SerializeField] Transform[] menuPrincipal;
        [SerializeField] internal GameSettings gameSettings;

        [Header("Menu Fades")]
        [SerializeField] protected Transform panelFade;
        [SerializeField] Animator fadeAnimator;
        protected float fadeInTime;
        protected float fadeOutTime;

        [Header("Menu SFX")]
        [SerializeField] AudioEventSample clickSound;

        protected int lastIndex;
        static readonly int FadeIn = Animator.StringToHash("FadeIn");
        static readonly int FadeOut = Animator.StringToHash("FadeOut");

        protected virtual void Awake()
        {
            fadeInTime = Utils.Tools.GetAnimationTime(fadeAnimator, "FadeIn");
            fadeOutTime = Utils.Tools.GetAnimationTime(fadeAnimator, "FadeOut");
            SetLocalization();
            //Debug.Log($"Tempos: {fadeInTime} , {fadeOutTime}");
        }
        #region Actions Application
        protected virtual void OnApplicationFocus(bool hasFocus)
        {
            Time.timeScale = hasFocus ? 1 : 0;
        }

        protected virtual void OnApplicationPause(bool pauseStatus)
        {
            Time.timeScale = pauseStatus ? 0 : 1;
        }
        #endregion
        protected void SetInternalMenu(int indexStart = 0)
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


        protected void PerformFadeOut()
        {
            fadeAnimator.SetTrigger(FadeOut);
        }
        protected void PerformFadeIn()
        {
            fadeAnimator.SetTrigger(FadeIn);
        }
        public void PlayClickSfx()
        {
            GameAudioManager.instance.PlaySfx(clickSound);
        }
        protected void LoadSceneHub()
        {
            SceneManager.LoadScene("HUB");
        }
        protected void LoadSceneGamePlay()
        {
            SceneManager.LoadScene("GamePlay");
        }
    }
}
