using System;
using System.Collections.Generic;
using NewRiverAttack.LevelBuilder;
using NewRiverAttack.SaveManagers;
using UnityEngine;
using UnityEngine.UI;

namespace NewRiverAttack.HUBManagers.UI
{
    public class UiHubIcons: MonoBehaviour
    { 
        
        private Image _missionIcon;
        private int _hubOrder;
        private LevelData _level;
        private HubGameManager _hubGameManager;

        private void OnEnable()
        {
            SetInitialReferences();
            _hubGameManager.EventInitializeHub += InitializeIcon;
            _hubGameManager.EventCursorUpdateHub += InitializeIcon;
        }
        

        private void SetInitialReferences()
        {
            _missionIcon = GetComponentInChildren<Image>();
            _hubGameManager = HubGameManager.Instance;
        }

        private void OnDisable()
        {
            _hubGameManager.EventInitializeHub -= InitializeIcon;
            _hubGameManager.EventCursorUpdateHub -= InitializeIcon;
        }

        public void SetIcon(LevelData levelData, int hubIndex)
        {
            _level = levelData;
            _missionIcon.sprite = _level.hudPath.iconSprite;
            _hubOrder = hubIndex;
        }
        private void InitializeIcon(List<HubOrderData> hubOrderData, int startIndex)
        {
            if (hubOrderData[startIndex].levelData.hudPath.levelsStates != LevelsStates.Complete)
            {
                hubOrderData[_hubOrder].levelData.hudPath.levelsStates = LevelsStates.Locked;
                if (_hubOrder < GameOptionsSave.instance.activeIndexMissionLevel)
                {
                    hubOrderData[_hubOrder].levelData.hudPath.levelsStates = LevelsStates.Open;
                }
                if (startIndex == _hubOrder)
                {
                    hubOrderData[startIndex].levelData.hudPath.levelsStates = LevelsStates.Actual;
                }
            }
            SetColorState(hubOrderData[_hubOrder].levelData.hudPath);
        }
        
        private void SetColorState(HubData hubData)
        {
            _missionIcon.color = hubData.levelsStates switch
            {
                LevelsStates.Locked => _hubGameManager.LockedColor,
                LevelsStates.Actual => _hubGameManager.ActualColor,
                LevelsStates.Complete => _hubGameManager.CompleteColor,
                LevelsStates.Open => _hubGameManager.OpenColor,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
