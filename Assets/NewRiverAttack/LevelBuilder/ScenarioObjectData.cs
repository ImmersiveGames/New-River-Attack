using UnityEngine;

namespace NewRiverAttack.LevelBuilder
{
    [System.Serializable]
    public struct ScenarioObjectData
    {
        public GameObject segmentObject;
        public GameObject enemySetObject; // GameObject do conjunto de inimigos
        public float absolutePosition;
        public LevelTypes levelType;

        public ScenarioObjectData(GameObject segmentObject, GameObject enemySetObject, float absolutePosition,LevelTypes levelType)
        {
            this.segmentObject = segmentObject;
            this.enemySetObject = enemySetObject;
            this.absolutePosition = absolutePosition;
            this.levelType = levelType;
        }
    }
}