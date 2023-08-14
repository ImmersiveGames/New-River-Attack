using UnityEngine;
namespace RiverAttack
{
    [System.Serializable]
    public class EnemiesResults
    {
        [SerializeField]
        public EnemiesScriptable enemy;
        [SerializeField]
        public int quantity;
        public enum CollisionType
        {
            Shoot, Bomb, Collider, Collected
        }
        public CollisionType collisionType;

        public EnemiesResults(EnemiesScriptable enemy, int quantity, CollisionType collision)
        {
            this.enemy = enemy;
            this.quantity = quantity;
            this.collisionType = collision;
        }

        public int scoreTotal
        {
            get { return this.quantity * enemy.enemyScore; }
        }
    }
}

    
