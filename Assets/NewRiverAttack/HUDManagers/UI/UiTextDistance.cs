using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.PlayerManagers.ScriptableObjects;
using TMPro;
using UnityEngine;

namespace NewRiverAttack.HUDManagers.UI
{
    public class UiTextDistance : MonoBehaviour
    {
        [SerializeField] private int playerIndex;
        private GameHudManager _gameHudManager;
        private PlayerSettings _playerSettings;
        private TMP_Text _textDistance;

        #region Unity Methods

        private void OnEnable()
        {
            SetInitialReferences();
            _gameHudManager.EventHudDistanceUpdate += UpdateDistance;
        }

        private void Start()
        {
            UpdateDistance(0, playerIndex);
        }

        private void OnDisable()
        {
            _gameHudManager.EventHudDistanceUpdate -= UpdateDistance;
        }

        #endregion

        private void SetInitialReferences()
        {
            _gameHudManager = GameHudManager.Instance;
            _textDistance = GetComponent<TMP_Text>();
        }

        private void UpdateDistance(int distance, int indexPlayer)
        {
            if (playerIndex == indexPlayer)
            {
                _textDistance.text = distance.ToString();
            }
        }
    }
}