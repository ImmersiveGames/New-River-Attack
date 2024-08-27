using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using NewRiverAttack.PlayerManagers.ScriptableObjects;

namespace NewRiverAttack.GamePlayManagers.GamePlayLogs
{
    [System.Serializable]
    public class GameStatisticHit
    {
        public PlayerSettings player;
        public ObjectsScriptable enemy;
        public int quantity;
        public EnumCollisionType collisionType;
        
        public GameStatisticHit(PlayerSettings player, ObjectsScriptable enemy, int quantity, EnumCollisionType collision)
        {
            this.player = player;
            this.enemy = enemy;
            this.quantity = quantity;
            collisionType = collision;
        }
    }
}