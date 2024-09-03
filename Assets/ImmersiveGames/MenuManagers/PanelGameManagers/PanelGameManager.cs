﻿using System;
using ImmersiveGames.InputManager;
using NewRiverAttack.GamePlayManagers;
using UnityEngine;

namespace ImmersiveGames.MenuManagers.PanelGameManagers
{
    public sealed class PanelGameManager : MonoBehaviour
    {
        private GamePlayManager _gamePlayManager;

        #region Delegates

        public delegate void PanelGameHandle();

        public event PanelGameHandle EventPauseGame;
        public event PanelGameHandle EventUnPauseGame;
        public event PanelGameHandle EventHudGame;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            SetInitialReferences();
            _gamePlayManager.EventGameReady += SetHudMenu;
            
        }

        private void SetHudMenu()
        {
            //InputGameManager.ActionManager.ActivateActionMap(ActionManager.GameActionMaps.Player);
        }

        private void OnDisable()
        {
            _gamePlayManager.EventGameReady -= SetHudMenu;
        }

        #endregion
        
        private void SetInitialReferences()
        {
            _gamePlayManager = GamePlayManager.instance;
        }
        
        
        #region Call Events

        private void OnEventPauseGame()
        {
            EventPauseGame?.Invoke();
        }
        private void OnEventUnPauseGame()
        {
            EventUnPauseGame?.Invoke();
        }
        private void OnEventHudGame()
        {
            EventHudGame?.Invoke();
        }
        #endregion


        
    }
}