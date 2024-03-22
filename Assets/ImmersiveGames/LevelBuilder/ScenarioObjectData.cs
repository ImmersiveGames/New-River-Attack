using UnityEngine;

namespace ImmersiveGames.LevelBuilder
{
    [System.Serializable]
    public class ScenarioObjectData
    {
        public GameObject segmentObject;
        public GameObject enemySetObject; // GameObject do conjunto de inimigos
        public float absolutePosition;

        public ScenarioObjectData(GameObject segmentObject, GameObject enemySetObject, float absolutePosition)
        {
            this.segmentObject = segmentObject;
            this.enemySetObject = enemySetObject;
            this.absolutePosition = absolutePosition;
        }

        public void RemoveObjectScene()
        {
            // Limpar os inimigos antes de destruir o conjunto de inimigos
            if (enemySetObject != null)
            {
                CleanupEnemies();
                Object.Destroy(enemySetObject);
            }

            // Destruir o trackObject
            if (segmentObject != null)
            {
                Object.Destroy(segmentObject);
            }
        }

        private void CleanupEnemies()
        {
            // Verificar se o enemySetObject possui o componente EnemyBehavior
            /*EnemyBehavior[] enemies = enemySetObject.GetComponentsInChildren<EnemyBehavior>();
            foreach (var enemy in enemies)
            {
                // Parar qualquer comportamento em execução nos inimigos
                enemy.StopBehavior(); // Método fictício para parar o comportamento do inimigo
            }*/
        }
    }
}