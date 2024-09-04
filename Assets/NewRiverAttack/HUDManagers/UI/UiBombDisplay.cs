using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NewRiverAttack.HUDManagers.UI
{
    public class UiBombDisplay : MonoBehaviour
    {
        public int indexPlayer = 0;
        public TMP_Text tmpTextBomb;
        public Image bombOn;
        public Image bombOff;

        private GamePlayManager _gamePlayManager;
        private PlayerBombs _playerBombs;

        #region UNITYMETHODS

        private void OnEnable()
        {
            _gamePlayManager = GamePlayManager.Instance;
            _gamePlayManager.EventHudBombUpdate += UpdateBombs;
        }

        private void Start()
        {
            SetInitialReferences();
            UpdateBombs(_playerBombs.GetBomb, indexPlayer);
        }

        private void OnDisable()
        {
            _gamePlayManager.EventHudBombUpdate -= UpdateBombs;
        }

        #endregion

        private void SetInitialReferences()
        {
            
            var playerMaster = _gamePlayManager.GetPlayerMaster(indexPlayer);
            _playerBombs = playerMaster.GetComponent<PlayerBombs>();
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