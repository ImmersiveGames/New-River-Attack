using System;
using System.Collections;
using ImmersiveGames;
using ImmersiveGames.CameraManagers;
using ImmersiveGames.DebugManagers;
using NewRiverAttack.GameManagers;
using NewRiverAttack.HUBManagers;
using NewRiverAttack.SaveManagers;
using NewRiverAttack.StateManagers;
using NewRiverAttack.StateManagers.States;
using UnityEngine;

namespace NewRiverAttack.GamePlayManagers
{
    public sealed class GamePlayManager : MonoBehaviour
    {
        #region Actions
        public event Action EventGameReadyGo;
        public event Action EventGameFinisher;
        public event Action EventGameResetClear;
        public event Action EventGameReset; //Hard Reset
        public event Action EventObstacleReload; //SoftReload (Respawn)
        public event Action EventPostStateGameInitialize;
        public event Action EventGameOver;
        public event Action EventGamePause;
        public event Action EventGameUnPause;

        #endregion
        
        public static GamePlayManager Instance { get; private set; }

        #region Unity Methods
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                
                DebugManager.Log<GamePlayManager>("GamePlayManager instanciado.");
            }
            else
            {
                Destroy(gameObject);
                DebugManager.LogWarning<GamePlayManager>("Tentativa de criar uma segunda instância de GamePlayManager foi evitada.");
            }
        }
        

        private void Start()
        {
            IsPause = false;
            StartCoroutine(WaitForInitialization());
        }

        private void OnDisable()
        {
            GameSaveHandler.Instance.SaveGameData();
        }

        private void OnApplicationQuit()
        {
            GameSaveHandler.Instance.SaveGameData();
        }

        #endregion

        #region Controle de Jogo

        public bool ShouldBePlayingGame =>
            GameManager.StateManager.GetCurrentState is GameStatePlay && PlayersManager.Instance.HasPlayersActive && !IsPause;

        public bool IsPause { get; private set; }

        #endregion

        //Chamado Na Animação pós o texto de "GO"
        public void StartReadyGame()
        {
            OnEventGameReadyGo();
        }
        public void FinisherGame()
        {
            AudioManager.instance.PlayBGMOneShot("Finish");
            OnEventGameFinisher();
        }

       internal void SendTo(GamePlayModes gamePlayModes)
        {
            switch (gamePlayModes)
            {
                case GamePlayModes.MissionMode:
                    GameManager.instance.ActiveLevel.hudPath.levelsStates = LevelsStates.Complete;
                    Invoke(nameof(SendToHub), 2f);
                    break;
                case GamePlayModes.ClassicMode:
                    Invoke(nameof(SendToCompleteGame), 2f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private async void SendToHub()
        {
            await GameManager.StateManager.ChangeStateAsync(StatesNames.GameStateHub.ToString()).ConfigureAwait(false);
        }
        
        private async void SendToCompleteGame()
        {
            await GameManager.StateManager.ChangeStateAsync(StatesNames.GameStateEndGame.ToString()).ConfigureAwait(false);
        }

        #region Métodos Auxiliares
        

        private IEnumerator WaitForInitialization()
        {
            while (!GameManager.StateManager.GetCurrentState.StateFinalization)
            {
                yield return null;
            }
            DebugManager.Log<GamePlayManager>("Inicia o jogo");
            OnEventPostStateGameInitialize();
            //Aqui são as configurações assim que a cena for totalmente carregada.
        }
        
        #endregion

        #region Calls

        private void OnEventPostStateGameInitialize()
        {
            EventPostStateGameInitialize?.Invoke();
        }
        internal void OnEventGameReadyGo()
        {
            EventGameReadyGo?.Invoke();
        }
        
        internal void OnEventGamePause()
        {
            IsPause = true;
            EventGamePause?.Invoke();
            Time.timeScale = 0;
        }
        internal void OnEventGameUnPause()
        {
            IsPause = false;
            EventGameUnPause?.Invoke();
            Time.timeScale = 1;
        }
        internal void OnEventGameOver()
        {
            GameSaveHandler.Instance.SaveGameData();
            EventGameOver?.Invoke();
        }
        

        private void OnEventGameFinisher()
        {
            GameSaveHandler.Instance.SaveGameData();
            EventGameFinisher?.Invoke();
        }
        
        internal void OnEventGameReset()
        {
            IsPause = false;
            CameraManager.ActiveEndCamera(false);
            CameraManager.ActiveStartCamera();
            GameManager.StateManager.ForceChangeState(StatesNames.GameStatePlay.ToString());
            GameSaveHandler.Instance.SaveGameData();
            EventGameReset?.Invoke();
        }
        internal void OnEventObstacleReload()
        {
            EventObstacleReload?.Invoke();
        }
        internal void OnEventGameResetClear()
        {
            EventGameResetClear?.Invoke();
        }
        #endregion
        
    }
}
