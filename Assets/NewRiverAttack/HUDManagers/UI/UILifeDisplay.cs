using ImmersiveGames.DebugManagers;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;
using UnityEngine.UI;
using GamePlayManager = NewRiverAttack.GamePlayManagers.GamePlayManager;

namespace NewRiverAttack.HUDManagers.UI
{
    public class UILifeDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject iconLives;
        [SerializeField] private int playerIndex = 0;
        private GamePlayManager _gamePlayManager;
        private PlayerMaster _playerMaster;
        private PlayerLives _playerLives;

        #region UNITYMETHODS

        private void OnEnable()
        {
            SetInitialReferences();

            _gamePlayManager.EventHudLivesUpdate += SetLivesUI;
        }

        private void Start()
        {
            CreateLiveIcon(transform, _playerLives.GetLives);
        }

        private void OnDisable()
        {
            _gamePlayManager.EventHudLivesUpdate -= SetLivesUI;
        }

        private void SetLivesUI(int valueUpdate, int iPlayerIndex)
        {
            if (_playerMaster.PlayerIndex != iPlayerIndex) return;
            DebugManager.Log<UILifeDisplay>($"UPDATE LIVES: {valueUpdate}");
            CreateLiveIcon(transform, valueUpdate);
        }

        #endregion

        private void SetInitialReferences()
        {
            _gamePlayManager = GamePlayManager.Instance;
            _playerMaster = _gamePlayManager.GetPlayerMaster(playerIndex);
            _playerLives = _playerMaster.GetComponent<PlayerLives>();
        }

        private void CreateLiveIcon(Transform parent, int quantity)
        {
            // Garante que a quantidade não seja menor que 0
            quantity = Mathf.Max(quantity, 0);

            // Obtém o número atual de filhos
            int childCount = parent.childCount;

            // Calcula a diferença entre a quantidade desejada e o número atual de filhos
            int diff = quantity - childCount;

            // Se a diferença for negativa, remova os filhos extras
            if (diff < 0)
            {
                for (int i = childCount - 1; i >= quantity; i--)
                {
                    Destroy(parent.GetChild(i).gameObject);
                }
            }
            // Se a diferença for positiva, adicione os filhos necessários
            else if (diff > 0)
            {
                for (int i = 0; i < diff; i++)
                {
                    var icon = Instantiate(iconLives, parent);
                    icon.GetComponent<Image>().sprite = _playerMaster.ActualSkin.GetSpriteLife();
                }
            }
        }
    }
}