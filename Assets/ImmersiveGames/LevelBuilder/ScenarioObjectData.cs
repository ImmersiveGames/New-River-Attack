using UnityEngine;

namespace ImmersiveGames.LevelBuilder
{
    [System.Serializable]
    public struct ScenarioObjectData
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
    }
}