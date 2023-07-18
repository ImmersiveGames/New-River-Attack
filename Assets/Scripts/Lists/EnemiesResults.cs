using System.Collections;
using System.Collections.Generic;
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

        public EnemiesResults(EnemiesScriptable enemy, int quantity)
        {
            this.enemy = enemy;
            this.quantity = quantity;
        }

        public int scoreTotal
        {
            get { return this.quantity * enemy.enemyScore; }
        }
    }
}
