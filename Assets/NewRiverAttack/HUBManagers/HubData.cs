using NewRiverAttack.LevelBuilder;
using UnityEngine;

namespace NewRiverAttack.HUBManagers
{
    [System.Serializable]
    public struct HubData
    {
        public GameObject segmentObject;
        public Sprite iconSprite;
        public LevelsStates levelsStates;
        public HubData(GameObject path, Sprite sprite, LevelsStates states)
        {
            segmentObject = path;
            iconSprite = sprite;
            levelsStates = LevelsStates.Locked;
        }
    }
}