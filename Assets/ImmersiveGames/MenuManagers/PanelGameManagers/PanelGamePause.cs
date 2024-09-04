﻿using NewRiverAttack.GameManagers;
using NewRiverAttack.GamePlayManagers;
using UnityEngine;

namespace ImmersiveGames.MenuManagers.PanelGameManagers
{
    public class PanelGamePause : MonoBehaviour
    {
        [SerializeField] private RectTransform hubButton;
        private void OnEnable()
        {
            hubButton.gameObject.SetActive(false);
            if(GameManager.instance.gamePlayMode == GamePlayModes.MissionMode)
                hubButton.gameObject.SetActive(true);
        }
    }
}