using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;
using Utils;
using Object = UnityEngine.Object;
namespace RiverAttack
{
    public abstract class GameState
    {
        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void ExitState();
    }

    public enum LevelTypes { Menu = 0, Hub = 1, Grass = 2, Forest = 3, Swamp = 4, Antique = 5, Desert = 6, Ice = 7, GameOver = 8, Complete = 9, HUD = 10 }
    public class GameManager : Singleton<GameManager>
    {
        public bool debugMode;
        bool m_OnTransition;
        public PanelManager startMenu;

        public float fadeDurationEnter = 0.5f;
        public float fadeDurationExit = 0.5f;

        [Header("Layer Settings")]
        public LayerMask layerPlayer;
        public LayerMask layerEnemies;
        public LayerMask layerCollection;
        public LayerMask layerWall;

        [Header("Player Settings")]
        public GameObject playerPrefab;
        public Vector3 spawnPlayerPosition;
        public List<PlayerSettings> playerSettingsList = new List<PlayerSettings>();
        [SerializeField] internal List<PlayerMaster> initializedPlayerMasters = new List<PlayerMaster>();

        [Header("Camera Settings"), SerializeField]
        CinemachineVirtualCamera virtualCamera;

        [Header("CutScenes Settings")]
        [SerializeField] PlayableDirector openCutDirector;

        #region UnityMethods
        void Start()
        {
            if (debugMode)
            {
                ChangeState(new GameStatePlayGame());
                return;
            }
            ChangeState(new GameStateMenu());
        }
        void Update()
        {
            currentGameState?.UpdateState();

        }
  #endregion
        public GameState currentGameState
        {
            get;
            private set;
        }
        internal void ChangeState(GameState newState)
        {
            if (m_OnTransition) return;
            if (currentGameState != null)
            {
                if (currentGameState is GameStateOpenCutScene)
                {
                    ChangeState(currentGameState, newState);
                }
                else
                {
                    m_OnTransition = true;
                    StartCoroutine(PerformStateTransition(currentGameState, newState));
                }

            }
            else
            {
                currentGameState = newState;
                currentGameState.EnterState();
            }
        }
        public void PauseGame()
        {
            ChangeState(new GameStatePause());
        }
        public void UnPauseGame()
        {
            ChangeState(new GameStatePlayGame());
        }

        public void ActivePlayers(bool active)
        {
            if (initializedPlayerMasters.Count <= 0) return;
            foreach (var playerMaster in initializedPlayerMasters)
            {
                playerMaster.gameObject.SetActive(active);
            }
        }
        public bool haveAnyPlayerInitialized
        {
            get { return initializedPlayerMasters.Count > 0; }
        }

        public void UnPausedMovementPlayers()
        {
            if (initializedPlayerMasters.Count <= 0) return;
            foreach (var playerMaster in initializedPlayerMasters)
            {
                playerMaster.playerMovementStatus = PlayerMaster.MovementStatus.None;
            }
        }

        void ChangeState(GameState actualState, GameState nextState)
        {
            actualState.ExitState();

            currentGameState = nextState;
            currentGameState.EnterState();
        }

        IEnumerator PerformStateTransition(GameState actualState, GameState nextState)
        {
            //Debug.Log($"Start Coroutine");
            // Implemente o efeito de fade out aqui
            startMenu.PerformFadeOut();

            yield return new WaitForSeconds(fadeDurationEnter);

            actualState.ExitState();
            yield return new WaitForSeconds(fadeDurationExit);
            // Implemente o efeito de fade in aqui
            startMenu.PerformFadeIn();

            currentGameState = nextState;
            currentGameState.EnterState();
            m_OnTransition = false;
        }

        public void InstantiatePlayers()
        {
            if (initializedPlayerMasters.Count != 0)
                return;
            var playerSettings = playerSettingsList[^1];
            var playerObject = Instantiate(playerPrefab, spawnPlayerPosition, quaternion.identity);
            playerObject.name = playerSettings.name;
            var playerMaster = playerObject.GetComponent<PlayerMaster>();
            playerMaster.SetPlayerSettingsToPlayMaster(playerSettings);
            initializedPlayerMasters.Add(playerMaster);
            // Atualiza a cutscene com o animator do jogador;
            ChangeBindingReference("Animation Track", playerMaster.GetPlayerAnimator());
            // Coloca o player como Follow da camra
            SetFollowVirtualCam(playerObject.transform);
        }

        void ChangeBindingReference(string track, Object animator)
        {
            foreach (var playableBinding in openCutDirector.playableAsset.outputs)
            {
                if (playableBinding.streamName != track)
                    continue;
                var bindingReference = openCutDirector.GetGenericBinding(playableBinding.sourceObject);

                if (bindingReference == null)
                {
                    // Substituir a referência nula pelo Animator desejado
                    openCutDirector.SetGenericBinding(playableBinding.sourceObject, animator);
                }
            }
        }

        public void PlayOpenCutScene()
        {
            openCutDirector.Play();
        }

        void SetFollowVirtualCam(Transform follow)
        {
            virtualCamera.Follow = follow;
        }

        #region Buttons Actions
        public void BtnNewGame()
        {
            ChangeState(new GameStateOpenCutScene(openCutDirector));
        }
        #endregion

    }
}
