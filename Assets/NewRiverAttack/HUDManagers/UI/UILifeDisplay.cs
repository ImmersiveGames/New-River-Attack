using ImmersiveGames.DebugManagers;
using NewRiverAttack.GamePlayManagers;
using UnityEngine;
using UnityEngine.UI;

namespace NewRiverAttack.HUDManagers.UI
{
    public class UILifeDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject iconLives;
        [SerializeField] private int playerIndex;
        private Sprite _iconSkin;
        private PlayersManager _playersManager;
        private GameHudManager _gameHudManager;

        #region UNITYMETHODS

        private void Awake()
        {
            SetInitialReferences();
        }

        private void OnEnable()
        {
            _gameHudManager.EventHudLivesUpdate += SetLivesUI;
            _iconSkin = _playersManager.GetPlayerMaster(playerIndex).ActualSkin.GetSpriteLife();
            CreateLiveIcon(transform, _playersManager.PlayersDefault.startLives);
        }

        private void OnDisable()
        {
            _gameHudManager.EventHudLivesUpdate -= SetLivesUI;
        }
        #endregion

        private void SetInitialReferences()
        {
            _playersManager = PlayersManager.Instance;
            _gameHudManager = GameHudManager.Instance;
        }
        private void SetLivesUI(int valueUpdate, int iPlayerIndex)
        {
            if (playerIndex != iPlayerIndex) return;
            DebugManager.Log<UILifeDisplay>($"UPDATE LIVES: {valueUpdate}");
            CreateLiveIcon(transform, valueUpdate);
        }
        private void CreateLiveIcon(Transform parent, int quantity)
        {
            // Garante que a quantidade não seja menor que 0
            quantity = Mathf.Max(quantity, 0);

            // Obtém o número atual de filhos
            var childCount = parent.childCount;

            // Calcula a diferença entre a quantidade desejada e o número atual de filhos
            var diff = quantity - childCount;

            switch (diff)
            {
                // Se a diferença for negativa, remova os filhos extras
                case < 0:
                {
                    for (var i = childCount - 1; i >= quantity; i--)
                    {
                        Destroy(parent.GetChild(i).gameObject);
                    }

                    break;
                }
                // Se a diferença for positiva, adicione os filhos necessários
                case > 0:
                {
                    for (var i = 0; i < diff; i++)
                    {
                        var icon = Instantiate(iconLives, parent);
                        icon.GetComponent<Image>().sprite = _iconSkin;
                    }

                    break;
                }
            }
        }
    }
}