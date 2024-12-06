using System;
using ImmersiveGames.Utils;
using UnityEngine;

namespace ImmersiveGames.MenuManagers
{
    public class PanelInitialize : MonoBehaviour
    {
        [SerializeField] private GameObject firstPanel;
        private void OnEnable()
        {
            firstPanel.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            Tools.ToggleChildren(transform, false);
        }
    }
}