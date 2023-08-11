using UnityEngine;
using UnityEngine.Events;

namespace RiverAttack
{
    [CreateAssetMenu(fileName = "GasStation", menuName = "RiverAttack/EffectArea", order = 5)]

    public class EffectAreaScriptable : EnemiesScriptable
    {
        [Header("Effect Area Settings")]
        public int maxCollectible;
        [SerializeField]
        public UnityEvent effectAreaActions;

        public void EffectAreaStart(PlayerSettings player)
        {
           // GamePlayPowerUps.target = player;
            effectAreaActions?.Invoke();
        }
    }
}


