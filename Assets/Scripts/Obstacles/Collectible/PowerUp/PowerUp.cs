using UnityEngine;
using UnityEngine.Events;

namespace RiverAttack
{
    [CreateAssetMenu(fileName = "PowerUp", menuName = "RiverAttack/PowerUp", order = 3)]
    public class PowerUp : ScriptableObject
    {
        public new string name;
        public float duration;
        public bool canAccumulateDuration;
        public bool canAccumulateEffects;
        // used to apply the Powerup of the Powerup
        [SerializeField]
        public UnityEvent startAction;
        [SerializeField]
        public UnityEvent endAction;

        public void PowerUpStart(PlayerSettings player)
        {
            GamePlayPowerUps.target = player;
            startAction?.Invoke();
        }
        public void PowerUpEnd(PlayerSettings player)
        {
            GamePlayPowerUps.target = player;
            endAction?.Invoke();
        }
    }
}
