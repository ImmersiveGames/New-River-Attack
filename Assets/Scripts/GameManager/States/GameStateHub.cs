﻿using System.Collections;
using UnityEngine;

namespace RiverAttack
{
    public class GameStateHub: GameState
    {
        const float TIME_TO_FADE_BGM = 0.1f;
        bool m_CheckCompleteLevel;
        public override IEnumerator OnLoadState()
        {
            GameManager.instance.gameModes = GameManager.GameModes.Mission;
            GameAudioManager.instance.ChangeBGM(LevelTypes.Hub, TIME_TO_FADE_BGM);
            yield return null;
        }
        public override void EnterState()
        {
            GameHubManager.instance.readyHub = true;
            //Debug.Log($"Entra no Estado: HUB");
        }
        public override void UpdateState()
        {
            if (GameManager.instance.onLoadScene) return;
            //Debug.Log($"Game HUB!");
            /*if(!m_CheckCompleteLevel)
                GameHubManager.instance.OnCheckCompleteLevel();
            m_CheckCompleteLevel = true;*/

        }
        public override void ExitState()
        {
            //Debug.Log($"Sai do Estado: HUB");
        }
    }
}
