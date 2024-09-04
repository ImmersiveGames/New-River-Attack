using ImmersiveGames.DebugManagers;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NewRiverAttack.HUDManagers.UI
{
    public class UiRapidFire : MonoBehaviour
    {
        public int playerIndex = 0;
        private GamePlayManager _gamePlayManager;
        
        [SerializeField] private Image rapidFireDisable;
        [SerializeField] private Image rapidFireEnable;
        [SerializeField] private TMP_Text rapidFireCounter;
        private const float Limit = 0.018f;
        private float _spendTimer;
        
        
        #region Unity Methods

        private void OnEnable()
        {
            SetInitialReferences();
            _gamePlayManager.EventHudRapidFireUpdate += UpdateTimer;
            _gamePlayManager.EventHudRapidFireEnd += StopTimer;
        }

        private void Start()
        {
            rapidFireDisable.gameObject.SetActive(true);
            rapidFireEnable.gameObject.SetActive(false);
            rapidFireCounter.gameObject.SetActive(true);
            rapidFireCounter.text = "00";
        }

        private void OnDisable()
        {
            _gamePlayManager.EventHudRapidFireUpdate -= UpdateTimer;
            _gamePlayManager.EventHudRapidFireEnd -= StopTimer;
        }

        #endregion
        private void SetInitialReferences()
        {
            _gamePlayManager = GamePlayManager.Instance;
        }
        
        private void UpdateTimer(float timer, int indexPlayer)
        {
            if (playerIndex != indexPlayer) return;
            switch (timer)
            {
                case > Limit when !rapidFireEnable.gameObject.activeSelf:
                    rapidFireEnable.gameObject.SetActive(true);
                    break;
                case <= Limit when rapidFireEnable.gameObject.activeSelf:
                    timer = 0;
                    rapidFireEnable.gameObject.SetActive(false);
                    break;
            }

            rapidFireCounter.text = timer.ToString("F2");
        }
        private void StopTimer(float timer, int indexPlayer)
        {
            if (playerIndex != indexPlayer) return;
            rapidFireEnable.gameObject.SetActive(false);
            rapidFireCounter.text = timer.ToString("F2");
        }
    }
}