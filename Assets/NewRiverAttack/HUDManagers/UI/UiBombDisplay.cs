using System;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NewRiverAttack.HUDManagers.UI
{
    public class UiBombDisplay : MonoBehaviour
    {
        public int indexPlayer;
        public TMP_Text tmpTextBomb;
        public Image bombOn;
        public Image bombOff;

  
        private PlayersManager _playersManager;
        private GameHudManager _gameHudManager;
        private PlayerBombs _playerBombs;

        #region UNITYMETHODS

        private void Awake()
        {
            SetInitialReferences();
        }

        private void OnEnable()
        {
            _gameHudManager.EventHudBombUpdate += UpdateBombs;
        }

        private void Start()
        {
            var playerMaster = _playersManager.GetPlayerMaster(indexPlayer);
            _playerBombs = playerMaster.GetComponent<PlayerBombs>();
            UpdateBombs(_playerBombs.GetBomb, indexPlayer);
        }

        private void OnDisable()
        {
            _gameHudManager.EventHudBombUpdate -= UpdateBombs;
        }

        #endregion

        private void SetInitialReferences()
        {
            _gameHudManager = GameHudManager.Instance;
            _playersManager = PlayersManager.Instance;
        }

        private void UpdateBombs(int valueUpdate, int playerIndex)
        {
            if (indexPlayer != playerIndex) return;
            bombOff.enabled = valueUpdate <= 0;
            bombOn.enabled = valueUpdate > 0;

            tmpTextBomb.text = $"X {valueUpdate}";
        }
    }
}