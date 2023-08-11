using System.Collections;
using UnityEngine;

namespace RiverAttack
{
    public class GamePlayStartLevel : MonoBehaviour
    {
        /*[SerializeField]
        GameObject panelStartText;
        [Header("Move Player Start")]
        [SerializeField]
        float inTime;
        [Header("GameOver Set")]
        [SerializeField]
        public string gameOverText;
        [SerializeField]
        AudioClip gameOverSound;
        [Header("Finish Path")]
        [SerializeField]
        public string victoryText;
        [SerializeField]
        AudioClip victorySound;
        [SerializeField]
        AudioClip successSound;
        [SerializeField]
        AudioClip failSound;

        GamePlayManager m_GamePlayManager;
        AudioSource m_AudioSource;
        Animator m_Animator;

        #region UNITYMETHODS
        void Awake()
        {
            m_Animator = panelStartText.GetComponent<Animator>();
            panelStartText.SetActive(false);
        }
        private void OnEnable()
        {
            SetInitialReferences();
            m_GamePlayManager.EventStartPath += StartLevel;
            m_GamePlayManager.EventCompletePath += EndLevel;
            m_GamePlayManager.EventUnPausePlayGame += ClosePanel;
            m_GamePlayManager.EventGameOver += GameOverScreen;
            panelStartText.SetActive(true);
        }
        private void OnDisable()
        {
            m_GamePlayManager.EventStartPath -= StartLevel;
            m_GamePlayManager.EventCompletePath -= EndLevel;
            m_GamePlayManager.EventUnPausePlayGame -= ClosePanel;
            m_GamePlayManager.EventGameOver -= GameOverScreen;
        }
  #endregion
        
        void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_AudioSource = GetComponent<AudioSource>();
        }

        void ClosePanel()
        {
            panelStartText.SetActive(false);
        }

        void StartLevel()
        {
            Time.timeScale = 1;
            SplashScreen(m_GamePlayManager.GetActualLevel().name, "Start");
            foreach (var player in m_GamePlayManager.listPlayer)
            {
                var playerPos = player.GetComponent<PlayerMaster>().GetPlayersSettings().spawnPosition;
                var to = new Vector3(playerPos.x, playerPos.y, m_GamePlayManager.GetActualLevel().levelMilestones[0].z);
                StartCoroutine(MoveToPosition(player.transform, to, inTime));
                player.GetComponent<PlayerMaster>().UpdateSavePoint(to);
            }
        }

        void GameOverScreen()
        {
            m_AudioSource.clip = gameOverSound;
            m_AudioSource.loop = false;
            m_AudioSource.Play();
            SplashScreen(gameOverText, "GameOver");
            m_AudioSource.PlayOneShot(failSound);
        }

        void EndLevel()
        {
            Debug.Log("END LEVEL SPLASH");
            SplashScreen(victoryText, "Finish");
            
            //TODO: Fazer controle de audio em outro arquivo
            
            m_AudioSource.PlayOneShot(successSound);
            m_AudioSource.clip = victorySound;
            m_AudioSource.loop = false;
            m_AudioSource.Play();
            var pos = m_GamePlayManager.GetActualLevel().levelMilestones[^1];
            var to = new Vector3(pos.x, pos.y + 10, pos.z);
            foreach (var player in m_GamePlayManager.listPlayer)
            {
                StartCoroutine(MoveToPosition(player.transform, to, inTime));
            }
            m_GamePlayManager.SaveAllPlayers();
        }

        void SplashScreen(string splashText, string paramAnimation)
        {
            //TODO: SplashScreen com texto
            Debug.Log("FAZ O MENU SPLASH NA TELA");
            panelStartText.SetActive(true);
            m_Animator.SetTrigger(paramAnimation);
            /*
            PanelSplashText splash;
            if (splash = panelStartText.GetComponent<PanelSplashText>())
                splash.SetSplashText(splashText);#1#
        }

        /*private void SplashScreen(LocalizationString splashText, string animeParm)
        {
            LocalizationTranslate translate = new LocalizationTranslate(LocalizationSettings.Instance.GetActualLanguage());
            string txt = translate.Translate(splashText, LocalizationTranslate.StringFormat.AllUpcase);
            SplashScreen(txt, animeParm);
        }#1#

        static IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove)
        {
            var currentPos = transform.position;
            float t = 0f;
            while (t < 1)
            {
                t += Time.deltaTime / timeToMove;
                transform.position = Vector3.Lerp(currentPos, position, t);
                yield return null;
            }
        }*/
    }
}
