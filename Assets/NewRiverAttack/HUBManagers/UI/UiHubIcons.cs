using NewRiverAttack.LevelBuilder;
using NewRiverAttack.SaveManagers;
using UnityEngine;
using UnityEngine.UI;

namespace NewRiverAttack.HUBManagers.UI
{
    public class UiHubIcons : MonoBehaviour
    {
        [Header("Icon Colors")]
        public Color lockedColor = Color.red;
        public Color openColor = Color.white;
        public Color actualColor = Color.yellow;
        
        private int _hubIndex; // Índice da ponte no HUB
        private LevelData _levelData;
        private Image _missionIcon; // Referência ao ícone visual
        
        private HubGameManager _hubGameManager;

        private void Awake()
        {
            _hubGameManager = HubGameManager.Instance;
        }

        private void OnEnable()
        {
            _hubGameManager.EventUpdateHub += UpdateIcon;
        }

        private void OnDisable()
        {
            _hubGameManager.EventUpdateHub -= UpdateIcon;
        }

        /// <summary>
        /// Configura o índice da ponte no HUB.
        /// </summary>
        public void SetIcon(LevelData levelData, int hubIndex)
        {
            _hubIndex = hubIndex;
            _levelData = levelData;
            SetSprite(levelData.hudPath.iconSprite);
            UpdateIcon(GameOptionsSave.Instance.activeIndexMissionLevel);
        }

        private void SetSprite(Sprite sprite)
        {
            _missionIcon = GetComponentInChildren<Image>();
            if (_missionIcon != null)
                _missionIcon.sprite = sprite;
        }
        
        private void UpdateIcon(int obj)
        {
            _levelData.hudPath.levelsStates = HubGameManager.UpdateLevel(obj, _hubIndex);
            SetColorState(_levelData.hudPath.levelsStates);
        }
        /// <summary>
        /// Define a cor do ícone com base no estado atual.
        /// </summary>
        private void SetColorState(LevelsStates state)
        {
            if (_missionIcon == null) return;
            _missionIcon.color = state switch
            {
                LevelsStates.Locked => lockedColor,
                LevelsStates.Open => openColor,
                LevelsStates.Actual => actualColor,
                _ => _missionIcon.color
            };
            
        }
        
    }
}
