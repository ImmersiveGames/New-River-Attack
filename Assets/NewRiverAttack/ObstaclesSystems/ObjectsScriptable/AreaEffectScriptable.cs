using GD.MinMaxSlider;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.ObjectsScriptable
{
    [CreateAssetMenu(fileName = "AreaEffect", menuName = "ImmersiveGames/RiverAttack/AreaEffect", order = 203)]
    public class AreaEffectScriptable : ObjectsScriptable
    {
        [Header("Movement Settings")] [Range(0, 50)]
        public float moveVelocity;

        [Header("Movement with Animation Curve")]
        public float animationDuration;

        public AnimationCurve animationCurve;

        [Header("Movement with Patrol")] [MinMaxSlider(0f, 100f)]
        public Vector2 approachMovement;
    }
}