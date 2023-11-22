using UnityEngine.SceneManagement;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.Serialization;


namespace RiverAttack
{
    [RequireComponent(typeof(AudioSource))]
    public class PanelBase : MonoBehaviour
    {
        [Header("Menus")]
        [SerializeField] protected Transform menuInitial;
        [SerializeField] protected Transform[] menuPrincipal;
        [SerializeField] CinemachineBrain cineMachineBrain;
        [SerializeField] CinemachineVirtualCameraBase[] menuCamera;
        [SerializeField] internal GameSettings gameSettings;        

        [Header("Menu SFX")]
        [SerializeField] AudioEventSample clickSound;

        

        protected int lastIndex;        

        protected virtual void Awake()
        {
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

        protected void SwitchCamera(int cameraIndex) 
        {
            foreach (CinemachineVirtualCameraBase virtualCam in menuCamera) 
            {
                virtualCam.Priority = 0;
                virtualCam.gameObject.SetActive(false);
            }

            menuCamera[cameraIndex].Priority = 10;            
            menuCamera[cameraIndex].gameObject.SetActive(true);            
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
