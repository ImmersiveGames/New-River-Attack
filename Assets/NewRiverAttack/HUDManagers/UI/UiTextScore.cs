using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using TMPro;
using UnityEngine;

namespace NewRiverAttack.HUDManagers.UI
{
    public class UiTextScore : MonoBehaviour
    {
        [SerializeField] private int playerIndex = 0;
        private GamePlayManager _gamePlayManager;
        private PlayerMaster _playerMaster;
        private TMP_Text _tmpTextScore;

        #region Unity Methods

        private void OnEnable()
        {
            SetInitialReferences();
            _gamePlayManager.EventHudScoreUpdate += UpdateScore;
        }

        private void Start()
        {
            UpdateScore(0, playerIndex);
        }

        private void OnDisable()
        {
            _gamePlayManager.EventHudScoreUpdate -= UpdateScore;
        }

        #endregion

        private void SetInitialReferences()
        {
            _gamePlayManager = GamePlayManager.instance;
            _tmpTextScore = GetComponent<TMP_Text>();
        }

        private void UpdateScore(int score, int indexPlayer)
        {
            if (playerIndex == indexPlayer)
            {
                _tmpTextScore.text = score.ToString();
            }
        }
    }
}