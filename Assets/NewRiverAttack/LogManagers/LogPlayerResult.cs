using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptables;
using NewRiverAttack.PlayerManagers.ScriptableObjects;

namespace NewRiverAttack.LogManagers
{
    [System.Serializable]
    public class LogPlayerResult
    {
        public PlayerSettings playerIndex;
        public ObjectsScriptable enemy;
        public int quantity;
        public EnumCollisionType collisionType;

        public LogPlayerResult(PlayerSettings player, ObjectsScriptable enemy, int quantity, EnumCollisionType collision)
        {
            playerIndex = player;
            this.enemy = enemy;
            this.quantity = quantity;
            collisionType = collision;
        }
    }
}