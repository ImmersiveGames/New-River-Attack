using UnityEngine;
using UnityEngine.Events;

namespace RiverAttack
{
    [CreateAssetMenu(fileName = "PowerUp", menuName = "RiverAttack/PowerUp", order = 3)]
    public class PowerUp : ScriptableObject
    {
        /*[Header("PowerUp Set")]
        [SerializeField]
        public new string name;
        [SerializeField]
        public float duration;

        [SerializeField]
        public bool canAccumulateEffects; // este powerup pode acumular com outros efeitos;

        [SerializeField]
        public bool canAccumulateDuration; // este power up acumula o tempo com outros iguais a ele;

        // used to apply the Powerup of the Powerup
        [SerializeField]
        public UnityEvent startAction;

        // used to remove the Powerup of the Powerup
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
        }*/
    }
}
