using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RiverAttack
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "RiverAttack/GameSettings", order = 0)]
    public class GameSettings : SingletonScriptableObject<GameSettings>
    {
        public GameObject playerPrefab;
        
        [Header("Options")]
        public float autoMovement;
        public int maxContinue;
        public int continues;
        
        [Header("Tag Names")]
        public string playerTag = "Players";        // identifica o nome da tag para o player
        public string wallTag = "Walls";            // identifica o nome da tag para as paredes
        public string enemyTag = "Enemies";         // identifica o nome da tag para os inimigos
        public string collectionTag = "Collection"; // identifica o nome da tag dos coletaveis
        
        public string shootTag = "Shoots"; // identifica o nome da tag dos disparos
    
        #region GameModes
        [SerializeField]
        public enum GameModes { Classic = 0, Mission = 1 }
        [SerializeField]
        GameModes actualGameMode = GameModes.Mission;
        
        public void ChangeGameMode(GameModes gameMode)
        {
            try
            {
                actualGameMode = gameMode;
            }
            catch (System.Exception)
            {
                Debug.LogError("Não existe esse modo de jogo na lista enum GameModes");
                throw;
            }
        }
        public void ChangeGameMode(int gameMode)
        {
            actualGameMode = (GameModes)gameMode;
        }
      
      #endregion

        #region GameScenes
        public enum GameScenes { Mission, Credits };
        [SerializeField]
        GameScenes actualGameScene = GameScenes.Mission;

        public GameScenes GetGameScene()
        {
            return actualGameScene;
        }
        public void ChangeGameScene(int sceneID)
        {
            try
            {
                actualGameScene = (GameScenes)sceneID;
            }
            catch (System.Exception)
            {
                actualGameScene = default(GameScenes);
                Debug.LogError("Não existe esse scena na lista enum GameScenes");
                throw;
            }
        }
  #endregion

        #region UnityMethods
        private void OnEnable()
        {
            // Aqui carrega apenas na primeira vez que o jogo carrega ja que só a uma entrada.
            // Carregando a referencia da primeira scene
            
            /*ChangeGameScene(SceneManager.GetActiveScene().buildIndex);
            previousScene = actualScene;*/
        }
  #endregion
    }
}
