using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.ObjectsScriptables
{
    [CreateAssetMenu(fileName = "Bridge", menuName = "ImmersiveGames/RiverAttack/Bridge", order = 204)]
    [System.Serializable]
    public class BridgeScriptable : ObjectsScriptable
    {
        [Header("Bridge Settings")]
        public bool isCheckPoint;
    }
}