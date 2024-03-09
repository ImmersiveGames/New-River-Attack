using System;
using UnityEngine;

namespace ImmersiveGames.MenuManagers.Interfaces
{
    public interface IMenuTimelineAnimation
    {
        MenuTimelineReference[] TimelineReferences { get; set; }
        float GetTimeAnimationByGameObject(GameObject menuGameObject);
        void TimelinePlayAnimation(float animationTimeStart);
    }

    [Serializable]
    public class MenuTimelineReference
    {
        public GameObject menuGameObject;
        public float timeAnimation;
    }
}