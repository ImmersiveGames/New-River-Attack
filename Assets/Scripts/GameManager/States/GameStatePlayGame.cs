﻿using UnityEngine;
namespace RiverAttack
{
    public class GameStatePlayGame: GameState
    {
        readonly GameManager m_GameManager;
        public GameStatePlayGame()
        {
            m_GameManager = GameManager.instance;
        }
        public override void EnterState()
        {
            m_GameManager.startMenu.SetMenuPrincipal(1,false);
            m_GameManager.startMenu.SetMenuHudControl(true);
            Debug.Log($"Entra no Estado: PlayGame");
            m_GameManager.ActivePlayers(true);
            m_GameManager.UnPausedMovementPlayers();
        }
        public override void UpdateState()
        {
            Debug.Log($"PlayGame!");
        }
        public override void ExitState()
        {
            m_GameManager.ActivePlayers(false);
            Debug.Log($"Sai do Estado: PlayGame");
        }
    }
}
