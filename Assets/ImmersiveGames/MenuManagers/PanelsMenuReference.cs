using System;
using Cinemachine;
using UnityEngine;

namespace ImmersiveGames.MenuManagers
{
    [Serializable]
    public struct PanelsMenuReference
    {
        public GameObject menuGameObject;
        public GameObject firstSelect;
        public float startTimelineAnimation;
        public CinemachineVirtualCameraBase virtualCameraBase;
    }
}