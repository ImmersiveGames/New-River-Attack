﻿using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.UI;
using Utils;
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

        [Header("Camera Settings"), SerializeField] internal CinemachineVirtualCamera virtualCamera;

        [Header("CutScenes Settings")]
        [SerializeField]
        internal PlayableDirector openCutDirector;
        [SerializeField]
        internal PlayableDirector endCutDirector;

        GamePlayManager m_GamePlayManager;
        PlayersInputActions m_InputSystem;

        #region UnityMethods
        void Awake()
        {
            m_InputSystem = new PlayersInputActions();
            m_InputSystem.Enable();
        }
        void OnEnable()
        {
            m_GamePlayManager = GamePlayManager.instance;
        }
        void Start()
        {
            m_InputSystem.Player.Pause.performed += ExecutePauseGame;
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

        #region  Actions Application
        void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                Time.timeScale = 1;
            }
            else
            {
                if (currentGameState is GameStatePlayGame)
                    startMenu.pauseButton.GetComponent<Button>().onClick.Invoke();
                Time.timeScale = 0;
            }
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                if (currentGameState is GameStatePlayGame)
                    startMenu.pauseButton.GetComponent<Button>().onClick.Invoke();
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
  #endregion
        public GameState currentGameState
        {
            get;
            private set;
        }
        internal void ChangeState(GameState newState)
        {
            //TODO: melhorar para fazer o state saltar as transições na entrada e na saida. talvez colocar na criação uma bolian para decidir
            if (m_OnTransition) return;
            if (currentGameState != null)
            {
                if (currentGameState is GameStateOpenCutScene)
                {
                    ChangeState(currentGameState, newState);
                }
                else
                {
                    if (newState is GameStateGameOver or GameStateEndCutScene)
                    {
                        ChangeState(currentGameState, newState);
                    }
                    else
                    {
                        m_OnTransition = true;
                        StartCoroutine(PerformStateTransition(currentGameState, newState));
                    }
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
        void ExecutePauseGame(InputAction.CallbackContext callbackContext)
        {
            switch (currentGameState)
            {
                case GameStatePlayGame:
                    startMenu.pauseButton.GetComponent<Button>().onClick.Invoke();
                    break;
                case GameStatePause:
                    startMenu.continueButton.GetComponent<Button>().onClick.Invoke();
                    break;
            }
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
            if (currentGameState is GameStateGameOver)
            {
                Invoke(nameof(GameOverState), .2f);
            }
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

        public void RemoveAllPlayers()
        {
            if (initializedPlayerMasters.Count <= 0) return;
            foreach (var playerMaster in initializedPlayerMasters)
            {
                DestroyImmediate(playerMaster.gameObject);
            }
            initializedPlayerMasters = new List<PlayerMaster>();
        }

        void GameOverState()
        {
            var curr = currentGameState as GameStateGameOver;
            curr?.GameOverState();
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
            Tools.ChangeBindingReference("Animation Track", playerMaster.GetPlayerAnimator(), openCutDirector);
            Tools.ChangeBindingReference("Animation Track", playerMaster.GetPlayerAnimator(), endCutDirector);
            // Coloca o player como Follow da camra
            Tools.SetFollowVirtualCam(virtualCamera, playerObject.transform);
        }

        public void PlayOpenCutScene()
        {
            Invoke(nameof(InvokePlayOpenCutScene),0.2f);
        }
        void InvokePlayOpenCutScene()
        {
            openCutDirector.Play();
        }
       

        #region Buttons Actions
        internal void BtnNewGame()
        {
            ChangeState(new GameStateOpenCutScene(openCutDirector));
        }

        internal void BtnGameRestart()
        {
            m_GamePlayManager.OnEventReSpawnEnemiesMaster();
            m_GamePlayManager.OnEventEnemiesMasterForceRespawn();
            m_GamePlayManager.OnEventUpdateRefugees(playerSettingsList[0].wealth);
            ChangeState(new GameStateMenu());
            GameMissionBuilder.instance.ResetBuildMission();
        }
        #endregion

    }
}
