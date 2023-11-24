using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Utils;
using UnityEngine.UI;

namespace RiverAttack
{
    public class GameLoadSceneManager : Singleton<GameLoadSceneManager>
    {
        [Header("Menu Fades")]
        public string nameSceneScreen;
       
        public void LoadSceneTransition(string sceneName) {
            StartCoroutine(UnloadSceneInAsync(sceneName));
        }
        
        private IEnumerator UnloadSceneInAsync(string nextScene)
        {
            string unloadScene = SceneManager.GetActiveScene().name;
            
            yield return SceneManager.LoadSceneAsync(nameSceneScreen, LoadSceneMode.Additive); // Carrega a cena de transição
            
            SceneManager.UnloadSceneAsync(unloadScene);
            while (SceneManager.GetSceneByName(unloadScene).isLoaded) {
                yield return null; // Aguarda até que a cena anterior seja totalmente descarregada
            }
            
            var panelSplashScreen = FindObjectOfType<PanelSplashScreen>();
            
            var loadScene = SceneManager.LoadSceneAsync(
                nextScene,
                LoadSceneMode.Additive
            );

            loadScene.allowSceneActivation = false;

            // wait for the scene to load
            while (!loadScene.isDone)
            {
                if (loadScene.progress >= 0.9f)
                {
                    //Se precisar load bar
                    
                    panelSplashScreen.PerformFadeIn();
                    break;
                }

                yield return null;
            }
            loadScene.allowSceneActivation = true;
            while (!loadScene.isDone)
            {
                yield return null;
            } 
            // Remove a cena de transição
            SceneManager.UnloadSceneAsync(nameSceneScreen);
        }
    }
}
