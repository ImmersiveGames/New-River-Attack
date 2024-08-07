using GD.MinMaxSlider;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.ObjectsScriptable
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "ImmersiveGames/RiverAttack/Enemies", order = 201)]
    [System.Serializable]
    public class EnemiesScriptable : ObjectsScriptable
    {
        [Header("Movement with Animation Curve")]
        public float animationDuration;
        public AnimationCurve animationCurve;
        
        [Header("Shoot Settings")] [Range(5f, 40f)]
        public float speedShoot;

        [Range(0.1f, 5f)] public float cadenceShoot;
        [Range(0, 10)] public int damageShoot;
        [MinMaxSlider(5f, 100f)] public Vector2 approachShoot;

        [Header("Drop Item Settings"), Range(0f, 1f)]
        public float dropItemChances = 0.25f;

        public EnemyDropData[] dropItems;
        
        
        public float GetShootApproach
        {
            get
            {
                return approachShoot.x switch
                {
                    0 when approachShoot.y > 0 => approachShoot.y,
                    > 0 => Random.Range(approachShoot.x, approachShoot.y),
                    _ => 0
                };
            }
        }
    }
}