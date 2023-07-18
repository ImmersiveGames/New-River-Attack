using System.Collections;
using UnityEngine;

namespace RiverAttack
{
    public class GamePlayStartLevel : MonoBehaviour
    {
        [SerializeField]
        private GameObject panelStartText;
        [Header("Move Player Start")]
        [SerializeField]
        private float inTime;
        [Header("GameOver Set")]
        [SerializeField]
        public string gameoverText;
        [SerializeField]
        private AudioClip gameoverSound;
        [Header("Finish Path")]
        [SerializeField]
        public string victoryText;
        [SerializeField]
        private AudioClip victorySound;
        [SerializeField]
        private AudioClip sucessSound;
        [SerializeField]
        private AudioClip failSound;

        private GamePlayManager playMaster;
        private AudioSource audioSource;
        private void Awake()
        {
            panelStartText.SetActive(false);
        }

        private void OnEnable()
        {
            SetInitialReferences();
            playMaster.EventStartPath += StartLevel;
            playMaster.EventCompletePath += EndLevel;
            playMaster.EventUnPausePlayGame += ClosePanel;
            playMaster.EventGameOver += GameOverScreen;
            //panelStartText.SetActive(true);

        }
        private void SetInitialReferences()
        {
            playMaster = GamePlayManager.instance;
            audioSource = GetComponent<AudioSource>();
        }

        private void ClosePanel()
        {
            panelStartText.SetActive(false);
        }

        private void StartLevel()
        {
            Time.timeScale = 1;
            SplashScreen(playMaster.GetActualLevel().name, "Start");
            //playMaster.ListPlayer[0].transform.GetComponent<PlayerMaster>().SetFixCam(true);
            foreach (var player in playMaster.ListPlayer)
            {
                Vector3 playerPos = player.GetComponent<PlayerMaster>().PlayersSettings().spawnPosition;
                Vector3 to = new Vector3(playerPos.x, playerPos.y, playMaster.GetActualLevel().levelMilstones[0].z);
                StartCoroutine(MoveToPosition(player.transform, to, inTime));
                player.GetComponent<PlayerMaster>().UpdateSavePoint(to);
            }
        }

        private void GameOverScreen()
        {
            audioSource.clip = gameoverSound;
            audioSource.loop = false;
            audioSource.Play();
            SplashScreen(gameoverText, "GameOver");
            audioSource.PlayOneShot(failSound);
        }

        private void EndLevel()
        {
            //Debug.Log("END LEVEL SPLASH");
            SplashScreen(victoryText, "Finish");
            //playMaster.ListPlayer[0].transform.GetComponent<PlayerMaster>().SetFixCam(false);
            //TODO: Fazer controle de audio em outro arquivo
            audioSource.PlayOneShot(sucessSound);
            audioSource.clip = victorySound;
            audioSource.loop = false;
            audioSource.Play();
            Vector3 pos = playMaster.GetActualLevel().levelMilstones[playMaster.GetActualLevel().levelMilstones.Count - 1];
            Vector3 to = new Vector3(pos.x, pos.y + 10, pos.z);
            foreach (GameObject player in playMaster.ListPlayer)
            {
                StartCoroutine(MoveToPosition(player.transform, to, inTime));
            }
            playMaster.SaveAllPlayers();
        }

        private void SplashScreen(string splashText, string animeParm)
        {
            //TODO: SplashScreen com texto
            /*panelStartText.SetActive(true);
            panelStartText.GetComponent<Animator>().SetTrigger(animeParm);
            PanelSplashText splash;
            if (splash = panelStartText.GetComponent<PanelSplashText>())
                splash.SetSplashText(splashText);*/
        }

        /*private void SplashScreen(LocalizationString splashText, string animeParm)
        {
            LocalizationTranslate translate = new LocalizationTranslate(LocalizationSettings.Instance.GetActualLanguage());
            string txt = translate.Translate(splashText, LocalizationTranslate.StringFormat.AllUpcase);
            SplashScreen(txt, animeParm);
        }*/

        public IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove)
        {
            var currentPos = transform.position;
            var t = 0f;
            while (t < 1)
            {
                t += Time.deltaTime / timeToMove;
                transform.position = Vector3.Lerp(currentPos, position, t);
                yield return null;
            }
        }

        private void OnDisable()
        {
            playMaster.EventStartPath -= StartLevel;
            playMaster.EventCompletePath -= EndLevel;
            playMaster.EventUnPausePlayGame -= ClosePanel;
            playMaster.EventGameOver -= GameOverScreen;
        }
    }
}
