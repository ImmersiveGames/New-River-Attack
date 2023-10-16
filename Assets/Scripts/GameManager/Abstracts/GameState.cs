using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace RiverAttack
{
    public abstract class GameState
    {
        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void ExitState();


        protected static GameManager gameManager;

        protected static void TryLoadScene(string nextSceneName)
        {
            gameManager = GameManager.instance;
            gameManager.loadSceneFinish = false;
            gameManager.StartCoroutine(LoadSceneAsync(nextSceneName));
        }
        static IEnumerator LoadSceneAsync(string nextSceneName)
        {
            gameManager.PerformFadeOut();
            var loadSceneAsync = SceneManager.LoadSceneAsync(nextSceneName);
            
            loadSceneAsync.allowSceneActivation = false;
            
            yield return new WaitForSeconds(gameManager.fadeOutTime);
            
            while (!loadSceneAsync.isDone)
            {
                // Verifique se a cena está 90% carregada.
                if (loadSceneAsync.progress >= 0.9f)
                {
                    loadSceneAsync.allowSceneActivation = true;
                    gameManager.PerformFadeIn();
                    yield return new WaitForSeconds(gameManager.fadeInTime);
                    gameManager.loadSceneFinish = true;
                }
                yield return null;
            }
        }
    }
    public enum LevelTypes { Menu = 0, Hub = 1, Grass = 2, Forest = 3, Swamp = 4, Antique = 5, Desert = 6, Ice = 7, GameOver = 8, Complete = 9, HUD = 10 }
}
