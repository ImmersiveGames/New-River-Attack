using System;
using UnityEngine;

namespace ImmersiveGames.MenuManagers.PanelGameManagers
{
    public class PanelGameComplete : MonoBehaviour
    {
        private void OnEnable()
        {
            AudioManager.instance.PlaySfx("SfxComplete");
        }
    }
}
