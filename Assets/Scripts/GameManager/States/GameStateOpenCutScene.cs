using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
namespace RiverAttack
{
    public class GameStateOpenCutScene : GameState
    {
        bool onScene;
        string nameSceneToLoad = "HUB";
        public override void EnterState()
        {
            Debug.Log($"Entra no Estado: CutScene");
            StartLoadScene(nameSceneToLoad);
        }

        public override void UpdateState()
        {
            if (onScene) return;
            Debug.Log($"CutScene!");
        }
        public override void ExitState()
        {
            Debug.Log($"Sai do Estado: CutScene");
        }
        
        
        void StartLoadScene(string nameScene)
        {
            nameSceneToLoad = nameScene;
            SceneManager.sceneLoaded += LoadedScene;
            SceneManager.LoadScene(nameSceneToLoad);
        }

        void LoadedScene(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != nameSceneToLoad)
                return;
            SceneManager.sceneLoaded -= LoadedScene; // Remova o evento após o carregamento
            Debug.Log("Cena carregada: " + scene.name);
            // Faça qualquer coisa que você precisa fazer após o carregamento da cena aqui
        }
        
        
        
       
        /*const float TIME_TO_FADE_BGM = 0.1f;
        const float TOLERANCE = 1f;
        readonly PlayableDirector m_PlayableDirector;
        readonly GameManager m_GameManager;
        readonly GamePlayManager m_GamePlayManager;

        internal GameStateOpenCutScene(PlayableDirector playableDirector)
        {
            m_PlayableDirector = playableDirector;
            m_GameManager = GameManager.instance;
            m_GamePlayManager = GamePlayManager.instance;
        }
        public override void EnterState()
        {
            //Debug.Log($"Entra no Estado: CutScene");
            m_GameManager.openCutDirector.gameObject.SetActive(true);
            if (!m_GamePlayManager.haveAnyPlayerInitialized)
                m_GamePlayManager.InstantiatePlayers();
            //m_GameManager.PlayOpenCutScene();
            //Iniciar a BGM
            GameAudioManager.instance.ChangeBGM(GamePlayManager.instance.actualLevels.bgmStartLevel, TIME_TO_FADE_BGM);
            //m_GameManager.startMenu.SetMenuPrincipal();
            //m_GameManager.startMenu.SetMenuHudControl(false);
        }

        public override void UpdateState()
        {
           // Debug.Log($"Rodando no Estado: CutScene");
            if (m_PlayableDirector == null) return;

            // Verificar se a animação já terminou
            if (!(m_PlayableDirector.time >= m_PlayableDirector.duration - TOLERANCE))
                return;
            m_GameManager.ChangeState(new GameStatePlayGame());
        }
        public override void ExitState()
        {
            //Debug.Log($"Saindo no Estado: CutScene");
        }*/

    }
}
