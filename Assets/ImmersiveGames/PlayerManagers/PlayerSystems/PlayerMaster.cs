using System;
using ImmersiveGames.GamePlayManagers;
using ImmersiveGames.PlayerManagers.ScriptableObjects;
using ImmersiveGames.SaveManagers;
using UnityEngine;

namespace ImmersiveGames.PlayerManagers.PlayerSystems
{
    public class PlayerMaster : MonoBehaviour
    {
        public bool godMode;
        #region Player Config Settings (privates)

        public int PlayerIndex { get; private set; }
        private PlayerSettings _playerSettings;
        private Animator _animator;
        
        private int _bombsMax;
        private int _livesMax;
        private int _fuelMax;

        private int _refugies;
        private int _score;
        private int _distance;

        private bool isDead;
        private bool isDisable;

        #endregion

        #region Delagates

        public delegate void PlayerMasterEventHandler(int indexPlayer, PlayersDefaultSettings defaultSettings);
        public event PlayerMasterEventHandler EventPlayerMasterInitialize;

        #endregion

        public bool PlayerIsReady => !isDead && !isDisable;

        #region Unity Methods

        private void Awake()
        {
            SetInitialReferences();
        }

        #endregion

        #region Metodos Auxiliares

        public Animator GetPlayerAnimator()
        {
            return _animator;
        }

        public GameObject GetPlayerSkin()
        {
            return _playerSettings.actualSkin.prefabSkin;
        }

        #endregion

        #region Initializations

        private void SetInitialReferences()
        {
            _animator = GetComponent<Animator>();
        }

        private void SetPlayerSettings(int indexPlayer, PlayersDefaultSettings defaultSettings)
        {
            _playerSettings = defaultSettings.playerSettings[indexPlayer];
            PlayerIndex = indexPlayer;
            _bombsMax = defaultSettings.maxBombs;
            _livesMax = defaultSettings.maxLives;
            _fuelMax = defaultSettings.maxFuel;

            var gamePlayMode = GameManager.instance.gamePlayMode;
            var gameOptionSave = GameOptionsSave.instance;
            //defaultSettings.SetSkinToPlayer(indexPlayer, null);
            var minLives = gameOptionSave.missionLives > 0 ? gameOptionSave.missionBombs : 1;
            
            /*_bombs = gamePlayMode == GamePlayModes.ClassicMode ? defaultSettings.startBombs : gameOptionSave.missionBombs;
            _lives = gamePlayMode == GamePlayModes.ClassicMode ? defaultSettings.startLives : minLives;*/
            
            _refugies = gameOptionSave.wallet;
            _distance = _score = 0;
            
            isDisable = isDead = false;
        }

        public void OnEventPlayerMasterInitialize(int indexPlayer, PlayersDefaultSettings defaultSettings)
        {
            SetPlayerSettings(indexPlayer, defaultSettings);
            EventPlayerMasterInitialize?.Invoke(indexPlayer, defaultSettings);
        }

        #endregion
        
    }
}